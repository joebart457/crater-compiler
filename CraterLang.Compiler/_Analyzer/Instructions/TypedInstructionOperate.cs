using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Enums;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal class TypedInstructionOperate: BaseTypedInstruction
    {
        public CrateType ResultingType { get; private set; }
        public OperatorType Operator { get; private set; }
        public BaseTypedValueTarget? Lhs { get; private set; }
        public BaseTypedValueTarget Rhs { get; private set; }

        public TypedInstructionOperate(CrateType resultingType, OperatorType @operator, BaseTypedValueTarget? lhs, BaseTypedValueTarget rhs)
        {
            ResultingType = resultingType;
            Operator = @operator;
            Lhs = lhs;
            Rhs = rhs;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
