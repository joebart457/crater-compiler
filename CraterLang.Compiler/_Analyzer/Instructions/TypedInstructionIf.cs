using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal class TypedInstructionIf: BaseTypedInstruction
    {
        public BaseTypedValueTarget Condition { get; private set; }
        public List<BaseTypedInstruction> InstructionsToExecute { get; private set; }
        public TypedInstructionIf(BaseTypedValueTarget condition, List<BaseTypedInstruction> instructionsToExecute)
        {
            Condition = condition;
            InstructionsToExecute = instructionsToExecute;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
