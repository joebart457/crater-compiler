using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal class TypedInstructionRet: BaseTypedInstruction
    {
        public BaseTypedValueTarget ReturnValue { get; private set; }

        public TypedInstructionRet(BaseTypedValueTarget returnValue)
        {
            ReturnValue = returnValue;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
