using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using TokenizerCore.Interfaces;
using CraterLang.Compiler._Parser.ValueTargets;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal class InstructionGet : BaseInstruction
    {
        public BaseValueTarget InstanceTarget { get; private set; }
        public IToken MemberSymbol { get; private set; }

        public InstructionGet(BaseValueTarget instanceTarget, IToken memberSymbol)
        {
            InstanceTarget = instanceTarget;
            MemberSymbol = memberSymbol;
        }

        public override BaseTypedInstruction Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
