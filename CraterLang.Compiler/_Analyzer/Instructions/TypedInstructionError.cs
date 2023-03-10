
using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal class TypedInstructionError: BaseTypedInstruction
    {
        public BaseTypedValueTarget ErrorToReturn { get; private set; }

        public TypedInstructionError(BaseTypedValueTarget errorToReturn)
        {
            ErrorToReturn = errorToReturn;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
