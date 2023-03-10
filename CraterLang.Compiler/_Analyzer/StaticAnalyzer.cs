using CraterLang.Compiler._Analyzer.Helpers;
using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer.LocationTargets;
using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Parser.Definitions;
using CraterLang.Compiler._Parser.Instructions;
using CraterLang.Compiler._Parser.LocationTargets;
using CraterLang.Compiler._Parser.ValueTargets;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using CraterLang.Compiler.Shared.Enums;
using TokenizerCore.Model;

namespace CraterLang.Compiler._Analyzer
{
    internal class StaticAnalyzer
    {
        private readonly AnalyzerContext _analyzerContext;

        public StaticAnalyzer(AnalyzerContext analyzerContext)
        {
            _analyzerContext = analyzerContext;
        }

        internal CrateMethod Determine(MethodDefinition methodDefinition)
        {
            var returnType = _analyzerContext.TypeProvider.ResolveToType(methodDefinition.ReturnType);
            var errorType = _analyzerContext.TypeProvider.ResolveToType(methodDefinition.ErrorType);
            if (!IsErrorType(errorType)) throw new Exception($"type {errorType} must implement error interface");
            var parameters = methodDefinition.Parameters.Select(p => new CrateMethodParameter(_analyzerContext.TypeProvider.ResolveToType(p.typeSymbol), p.name.Lexeme));
            _analyzerContext.SetEnclosingMethod(new CrateMethod(returnType, errorType, methodDefinition.MethodName.Lexeme, parameters.ToList()));
            foreach (var parameter in parameters)
                _analyzerContext.Environment.Define(new Token("TTWord", parameter.ParameterName, -1, -1), parameter.CrateType);

            var instructions = methodDefinition.Instructions.Select(ins => ins.Determine(this)).ToList();
            _analyzerContext.SetEnclosingMethod(null);
            var method = new UserDefinedMethod(returnType, errorType, methodDefinition.MethodName.Lexeme, parameters.ToList(), instructions);
            return method;
        }

        internal CrateType Determine(TypeDefinition typeDefinition)
        {
            var fieldDefinitions = typeDefinition.Fields.Select(f => new CrateField(_analyzerContext.TypeProvider.ResolveToType(f.typeSymbol), f.name.Lexeme));
            var type = new UserDefinedType(typeDefinition.TypeName.Lexeme);
            foreach(var field in fieldDefinitions)
            {
                if (type.Fields.Any(f => f.FieldName == field.FieldName)) throw new Exception($"redefinition of field {field.FieldName} in type {type.CType}");
                type.Fields.Add(field);
            }
            return type;
        }

        internal BaseTypedInstruction Determine(InstructionError instructionError)
        {
            if (!_analyzerContext.HasEnclosingMethod) throw new Exception($"invalid instrution: error; expect enclosing method");
            var errorToReturn = instructionError.ErrorToReturn.Determine(this);
            if (errorToReturn.CrateType != _analyzerContext.EnclosingMethod.ErrorType)
                throw new Exception($"expect error type of {_analyzerContext.EnclosingMethod.ErrorType} but got {errorToReturn.CrateType}");
            return new TypedInstructionError(errorToReturn);
        }

        internal BaseTypedInstruction Determine(InstructionGet instructionGet)
        {
            var instance = instructionGet.InstanceTarget.Determine(this);
            var field = instance.CrateType.GetField(instructionGet.MemberSymbol.Lexeme);
            _analyzerContext.UpdateRegister(RegisterType.Result, field.CrateType);
            return new TypedInstructionGet(instance, instructionGet.MemberSymbol);
        }

        internal BaseTypedInstruction Determine(InstructionIf instructionIf)
        {
            var condition = instructionIf.Condition.Determine(this);
            if (!IsTestable(condition.CrateType)) throw new Exception($"expect condition to resolve to boolean type");

            return new TypedInstructionIf(condition, instructionIf.InstructionsToExecute.Select(ins => ins.Determine(this)).ToList());
        }

        internal BaseTypedInstruction Determine(InstructionInvoke instructionInvoke)
        {
            var arguments = instructionInvoke.Arguments.Select(arg => arg.Determine(this));
            var argumentTypes = arguments.Select(arg => arg.CrateType).ToList();
            var method = _analyzerContext.MethodProvider.GetMethod(instructionInvoke.MethodName.Lexeme, argumentTypes);
            _analyzerContext.UpdateRegister(RegisterType.Result, method.ReturnType);
            _analyzerContext.UpdateRegister(RegisterType.Err, method.ErrorType);
            return new TypedInstructionInvoke(method, arguments.ToList());
        }

        internal BaseTypedInstruction Determine(InstructionLoopWhile instructionLoopWhile)
        {
            var condition = instructionLoopWhile.Condition.Determine(this);
            if (!IsTestable(condition.CrateType)) throw new Exception($"expect condition to resolve to boolean type");
            return new TypedInstructionLoopWhile(condition, instructionLoopWhile.InstructionsToExecute.Select(ins => ins.Determine(this)).ToList());
        }

        internal BaseTypedInstruction Determine(InstructionMov instructionMov)
        {
            var targetValue = instructionMov.Src.Determine(this);
            var targetLocation = instructionMov.Dest.Determine(this);
            if (targetValue.CrateType != targetLocation.CrateType) throw new Exception($"cannot move type {targetValue.CrateType} to location of type {targetLocation.CrateType}");
            return new TypedInstructionMov(targetLocation, targetValue);
        }

        internal BaseTypedInstruction Determine(InstructionOperate instructionOperate)
        {
            if (!Enum.TryParse<OperatorType>(instructionOperate.Operator.Lexeme, out var op)) 
                throw new Exception($"unsupported operator {instructionOperate.Operator.Lexeme}");
            var lhs = instructionOperate.Lhs?.Determine(this);
            var rhs = instructionOperate.Rhs.Determine(this);

            if (lhs == null)
            {
                if (!OperatorProvider.CanOperate(op, rhs.CrateType, out var unaryResultType)) throw new Exception($"operator {op} is not supported for type {rhs.CrateType}");
                _analyzerContext.UpdateRegister(RegisterType.OperatorResult, unaryResultType);
                return new TypedInstructionOperate(unaryResultType, op, null, rhs);
            }
            if (!OperatorProvider.CanOperate(op, lhs.CrateType, rhs.CrateType, out var binaryResultType)) 
                throw new Exception($"operator {op} is not supported for type {lhs.CrateType} with right-hand side type {rhs.CrateType}");
            _analyzerContext.UpdateRegister(RegisterType.OperatorResult, binaryResultType);
            return new TypedInstructionOperate(binaryResultType, op, lhs, rhs);

        }

        internal BaseTypedInstruction Determine(InstructionRet instructionRet)
        {
            if (!_analyzerContext.HasEnclosingMethod) throw new Exception($"invalid instrution: ret; expect enclosing method");
            var returnValue = instructionRet.ReturnValue.Determine(this);
            if (returnValue.CrateType != _analyzerContext.EnclosingMethod.ReturnType)
                throw new Exception($"expect return type of {_analyzerContext.EnclosingMethod.ReturnType} but got {returnValue.CrateType}");
            return new TypedInstructionRet(returnValue);           
        }

        internal BaseTypedInstruction Determine(InstructionSafeInvoke instructionSafeInvoke)
        {
            var arguments = instructionSafeInvoke.Arguments.Select(arg => arg.Determine(this));
            var argumentTypes = arguments.Select(arg => arg.CrateType).ToList();
            var method = _analyzerContext.MethodProvider.GetMethod(instructionSafeInvoke.MethodName.Lexeme, argumentTypes);
            _analyzerContext.UpdateRegister(RegisterType.Result, method.ReturnType);
            _analyzerContext.UpdateRegister(RegisterType.Err, method.ErrorType);
            return new TypedInstructionSafeInvoke(method, arguments.ToList());
        }

        internal BaseTypedInstruction Determine(InstructionStor instructionStor)
        {
            var targetValue = instructionStor.Src.Determine(this);
            if (instructionStor.Dest is LocationTargetVariable variable)
            {
                if (!_analyzerContext.Environment.Exists(variable.VariableName))
                {
                    _analyzerContext.Environment.Define(variable.VariableName, targetValue.CrateType);
                }
            }
            var targetLocation = instructionStor.Dest.Determine(this);
            if (targetValue.CrateType != targetLocation.CrateType) throw new Exception($"cannot store type {targetValue.CrateType} in location of type {targetLocation.CrateType}");
            return new TypedInstructionStor(targetLocation, targetValue);
        }


        internal BaseTypedLocationTarget Determine(LocationTargetVariable locationTargetVariable)
        {
            var targetType = _analyzerContext.Environment.Get(locationTargetVariable.VariableName);
            return new TypedLocationTargetVariable(targetType, locationTargetVariable.VariableName);
        }

        internal BaseTypedLocationTarget Determine(LocationTargetVirtualRegister locationTargetVirtualRegister)
        {
            var targetType = _analyzerContext.GetRegister(locationTargetVirtualRegister.Register);
            return new TypedLocationTargetVirtualRegister(targetType, locationTargetVirtualRegister.Register);
        }

        internal BaseTypedValueTarget Determine(ValueTargetLiteral valueTargetLiteral)
        {
            if (valueTargetLiteral.Value == null)
                return new TypedValueTargetLiteral(TypeTranslator.Translate(typeof(void)), null);
            return new TypedValueTargetLiteral(TypeTranslator.Translate(valueTargetLiteral.Value.GetType()), valueTargetLiteral.Value);
        }

        internal BaseTypedValueTarget Determine(ValueTargetSymbolAccessor valueTargetSymbolAccessor)
        {
            var targetType = _analyzerContext.Environment.Get(valueTargetSymbolAccessor.IdentifierSymbol);
            return new TypedValueTargetSymbolAccessor(targetType, valueTargetSymbolAccessor.IdentifierSymbol);
        }

        internal BaseTypedValueTarget Determine(ValueTargetVirtualRegister valueTargetVirtualRegister)
        {
            var resultingType = _analyzerContext.GetRegister(valueTargetVirtualRegister.Register);
            return new TypedValueTargetVirtualRegister(resultingType, valueTargetVirtualRegister.Register);
        }

        private bool IsTestable(CrateType type)
        {
            return type.CType == CTypes.bool_t;
        }

        private bool IsErrorType(CrateType type)
        {
            var hadErrorField = type.Fields.FirstOrDefault(f => f.FieldName == "had_error" && f.CrateType.CType == CTypes.bool_t);
            var errorMessageField = type.Fields.FirstOrDefault(f => f.FieldName == "error_message" && f.CrateType.CType == CTypes.string_t);
            return hadErrorField != null && errorMessageField != null;
        }

    }
}
