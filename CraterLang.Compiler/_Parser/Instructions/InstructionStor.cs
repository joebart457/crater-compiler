using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Parser.LocationTargets;
using CraterLang.Compiler._Parser.ValueTargets;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal class InstructionStor : BaseInstruction
    {
        public BaseLocationTarget Dest { get; private set; }
        public BaseValueTarget Src { get; private set; }
        public InstructionStor(BaseLocationTarget dest, BaseValueTarget src)
        {
            Dest = dest;
            Src = src;
        }

        public override BaseTypedInstruction Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
