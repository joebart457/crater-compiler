using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using TokenizerCore.Interfaces;
using CraterLang.Compiler._Parser.ValueTargets;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal class InstructionOperate : BaseInstruction
    {
        public IToken Operator { get; private set; }
        public BaseValueTarget? Lhs { get; private set; }
        public BaseValueTarget Rhs { get; private set; }

        public InstructionOperate(IToken @operator, BaseValueTarget? lhs, BaseValueTarget rhs)
        {
            Operator = @operator;
            Lhs = lhs;
            Rhs = rhs;
        }

        public override BaseTypedInstruction Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
