using CraterLang.Compiler._Analyzer.LocationTargets;
using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal class TypedInstructionMov: BaseTypedInstruction
    {
        public BaseTypedLocationTarget Dest { get; private set; }
        public BaseTypedValueTarget Src { get; private set; }
        public TypedInstructionMov(BaseTypedLocationTarget dest, BaseTypedValueTarget src)
        {
            Dest = dest;
            Src = src;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
