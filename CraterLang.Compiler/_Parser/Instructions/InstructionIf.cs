using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Parser.ValueTargets;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal class InstructionIf : BaseInstruction
    {
        public BaseValueTarget Condition { get; private set; }
        public List<BaseInstruction> InstructionsToExecute { get; private set; }
        public InstructionIf(BaseValueTarget condition, List<BaseInstruction> instructionsToExecute)
        {
            Condition = condition;
            InstructionsToExecute = instructionsToExecute;
        }

        public override BaseTypedInstruction Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
