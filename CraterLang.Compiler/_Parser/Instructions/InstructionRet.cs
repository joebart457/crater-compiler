using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Parser.ValueTargets;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal class InstructionRet : BaseInstruction
    {
        public BaseValueTarget ReturnValue { get; private set; }

        public InstructionRet(BaseValueTarget returnValue)
        {
            ReturnValue = returnValue;
        }

        public override BaseTypedInstruction Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
