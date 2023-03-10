using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal class TypedInstructionSafeInvoke: BaseTypedInstruction
    {
        public CrateMethod Method { get; private set; }
        public List<BaseTypedValueTarget> Arguments { get; private set; }
        public TypedInstructionSafeInvoke(CrateMethod method, List<BaseTypedValueTarget> arguments)
        {
            Method = method;
            Arguments = arguments;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
