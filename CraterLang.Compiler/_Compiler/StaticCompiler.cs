using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer.LocationTargets;
using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler.Constants;
using CraterLang.Compiler._Compiler.Helpers;
using CraterLang.Compiler._Compiler.Models;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Compiler
{
    internal class StaticCompiler
    {
        private readonly CompilerContext _context = new();

        public CompiledSource Compile(List<CrateType> types, List<CrateMethod> methods)
        {
            var source = new CompiledSource();
            foreach(var type in types)
            {
                _context.FlushRegisters();
                if (type is UserDefinedType udt)
                    source.ProvideType(udt, GenerateSource(udt));
                else HandleNativeType(type, source);
            }

            foreach(var method in methods)
            {
                _context.FlushRegisters();
                if (method is UserDefinedMethod udm)
                    source.ProvideMethod(udm, GenerateSource(udm));
                else HandleNativeMethod(method, source);
            }
            return source;
        }

        internal string GenerateSource(UserDefinedType type)
        {

            var sb = new StringBuilder();
            sb.AppendLine($"typedef struct {{");
            foreach(var field in type.Fields)
            {
                sb.AppendLine($"\t{field.CrateType.CType} {field.FieldName};");
            }
            sb.AppendLine($"}} {type.CType};");
            return sb.ToString();
        }

        internal void HandleNativeType(CrateType type, CompiledSource source)
        {
            if (type.CType == CTypes.string_t) source.ProvideHeader(CHeaders.string_impl_h);
            if (type.CType == CTypes.int16_t) source.ProvideHeader(CHeaders.stdint_h);
            if (type.CType == CTypes.int32_t) source.ProvideHeader(CHeaders.stdint_h);
            if (type.CType == CTypes.int64_t) source.ProvideHeader(CHeaders.stdint_h);
            if (type.CType == CTypes.uint16_t) source.ProvideHeader(CHeaders.stdint_h);
            if (type.CType == CTypes.uint32_t) source.ProvideHeader(CHeaders.stdint_h);
            if (type.CType == CTypes.uint64_t) source.ProvideHeader(CHeaders.stdint_h);
        }

        internal string GenerateSource(UserDefinedMethod method)
        {
            var sb = new StringBuilder();
            (var resultIdentifier, var errorResultIdentifier) = _context.GenerateEnclosingMethodIdentifiers(method);
            sb.Append($"RUNTIME_FN {method.MethodName}({method.ReturnType.CType}* {resultIdentifier}, {method.ErrorType.CType}* {errorResultIdentifier}");
            if (!method.Parameters.Any()) sb.AppendLine(")");
            else sb.AppendLine($", {string.Join(", ", method.Parameters.Select(p => $"{p.CrateType.CType} {p.ParameterName}"))})");
            sb.AppendLine("{");
            sb.AppendLine("\tint32_t rc = -1;");
            foreach(var ins in method.Instructions)
            {
                sb.AppendLine(ins.GenerateSource(this).Replace("\n", "\n\t"));
            }
            sb.AppendLine("\treturn -1;");
            sb.AppendLine("}");
            return sb.ToString();
        }

        internal void HandleNativeMethod(CrateMethod method, CompiledSource source)
        {
            // Pass
        }

        internal string GenerateSource(TypedValueTargetLiteral typedValueTargetLiteral)
        {
            if (typedValueTargetLiteral.CrateType.CType == CTypes.string_t)
            {
                return $"construct_string(\"{typedValueTargetLiteral.Value}\")";
            }
            if (typedValueTargetLiteral.Value == null) return "NULL";
            if (typedValueTargetLiteral.CrateType.CType == CTypes.bool_t) return $"{typedValueTargetLiteral.Value}".ToLowerInvariant();
            return $"{typedValueTargetLiteral.Value}";
        }

        internal string GenerateSource(TypedValueTargetSymbolAccessor typedValueTargetSymbolAccessor)
        {
            return $"{typedValueTargetSymbolAccessor.IdentifierSymbol.Lexeme}";
        }

        internal string GenerateSource(TypedValueTargetVirtualRegister typedValueTargetVirtualRegister)
        {
            return _context.GetRegister(typedValueTargetVirtualRegister.Register);
        }

        internal string GenerateSource(TypedInstructionError typedInstructionError)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"*{_context.EnclosingErrorResultIdentifier} = {typedInstructionError.ErrorToReturn.GenerateSource(this)};");
            sb.AppendLine("return 0;");
            return sb.ToString();
        }

        internal string GenerateSource(TypedInstructionGet typedInstructionGet)
        {
            var resultIdentifer = _context.GenerateResultIdentifier(typedInstructionGet.InstanceTarget.CrateType); //TODO use field type
            return $"{resultIdentifer} = {typedInstructionGet.InstanceTarget.GenerateSource(this)}.{typedInstructionGet.MemberSymbol.Lexeme};";
        }

        internal string GenerateSource(TypedInstructionIf typedInstructionIf)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"if ({typedInstructionIf.Condition.GenerateSource(this)})");
            sb.AppendLine("{");
            foreach(var ins in typedInstructionIf.InstructionsToExecute)
            {
                sb.AppendLine($"\t{ins.GenerateSource(this)}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        internal string GenerateSource(TypedInstructionInvoke typedInstructionInvoke)
        {
            (var resultIdentifier, var errorIdentifier) = _context.GenerateCallIdentifiers(typedInstructionInvoke.Method);
            var sb = new StringBuilder();
            sb.AppendLine($"{typedInstructionInvoke.Method.ErrorType.CType} {errorIdentifier};");
            sb.AppendLine($"{typedInstructionInvoke.Method.ReturnType.CType} {resultIdentifier};");
            sb.AppendLine($"rc = {typedInstructionInvoke.Method.MethodName}(&{resultIdentifier}, &{errorIdentifier}{(typedInstructionInvoke.Arguments.Any() ? ", " : "")} {string.Join(", ", typedInstructionInvoke.Arguments.Select(arg => arg.GenerateSource(this)))});");
            sb.AppendLine("if (rc != RUNTIME_SUCCESS) return rc;");
            return sb.ToString();
        }

        internal string GenerateSource(TypedInstructionLoopWhile typedInstructionLoopWhile)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"while ({typedInstructionLoopWhile.Condition.GenerateSource(this)})");
            sb.AppendLine("{");
            foreach (var ins in typedInstructionLoopWhile.InstructionsToExecute)
            {
                sb.AppendLine($"\t{ins.GenerateSource(this)}".Replace("\n", "\n\t"));
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        internal string GenerateSource(TypedInstructionMov typedInstructionMov)
        {
            return $"{typedInstructionMov.Dest.GenerateSource(this)} = {typedInstructionMov.Src.GenerateSource(this)};";
        }

        internal string GenerateSource(TypedInstructionOperate typedInstructionOperate)
        {
            string opString;
            if (typedInstructionOperate.Lhs == null)
            {
                
                var unaryOpeatorString = $"{UnaryOperatorSourceGenerator.GenerateOperator(typedInstructionOperate.Operator, typedInstructionOperate.Rhs.CrateType)}";
                opString = string.Format(unaryOpeatorString, typedInstructionOperate.Rhs.GenerateSource(this));
            }else
            {
                var opeatorString = $"{BinaryOperatorGenerator.GenerateOperator(typedInstructionOperate.Operator, typedInstructionOperate.Lhs.CrateType, typedInstructionOperate.Rhs.CrateType)}";
                opString = string.Format(opeatorString, typedInstructionOperate.Lhs.GenerateSource(this), typedInstructionOperate.Rhs.GenerateSource(this));
            }
            return $"{typedInstructionOperate.ResultingType.CType} {_context.GenerateOperatorResultIdentifier(typedInstructionOperate.Operator)} = {opString};";
        }

        internal string GenerateSource(TypedInstructionRet typedInstructionRet)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"*{_context.EnclosingResultIdentifier} = {typedInstructionRet.ReturnValue.GenerateSource(this)};");
            sb.AppendLine("return RUNTIME_SUCCESS;");
            return sb.ToString();
        }

        internal string GenerateSource(TypedInstructionSafeInvoke typedInstructionSafeInvoke)
        {
            (var resultIdentifier, var errorIdentifier) = _context.GenerateCallIdentifiers(typedInstructionSafeInvoke.Method);
            var sb = new StringBuilder();
            sb.AppendLine($"{typedInstructionSafeInvoke.Method.ErrorType.CType} {errorIdentifier};");
            sb.AppendLine($"{typedInstructionSafeInvoke.Method.ReturnType.CType} {resultIdentifier};");
            sb.AppendLine($"rc = {typedInstructionSafeInvoke.Method.MethodName}(&{resultIdentifier}, &{errorIdentifier}{(typedInstructionSafeInvoke.Arguments.Any() ? ", " : "")} {string.Join(", ", typedInstructionSafeInvoke.Arguments.Select(arg => arg.GenerateSource(this)))});");
            sb.AppendLine("if (rc != RUNTIME_SUCCESS) return rc;");
            return sb.ToString();
        }

        internal string GenerateSource(TypedInstructionStor typedInstructionStor)
        {
            return $"{typedInstructionStor.Dest.CrateType.CType} {typedInstructionStor.Dest.GenerateSource(this)} = {typedInstructionStor.Src.GenerateSource(this)};";
        }

        internal string GenerateSource(TypedLocationTargetVariable typedLocationTargetVariable)
        {
            return $"{typedLocationTargetVariable.VariableName.Lexeme}";
        }

        internal string GenerateSource(TypedLocationTargetVirtualRegister typedLocationTargetVirtualRegister)
        {
            return _context.GetRegister(typedLocationTargetVirtualRegister.Register);
        }
    }
}
