using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Compiler;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal class TypedInstructionGet: BaseTypedInstruction
    {
        public BaseTypedValueTarget InstanceTarget { get; private set; }
        public IToken MemberSymbol { get; private set; }

        public TypedInstructionGet(BaseTypedValueTarget instanceTarget, IToken memberSymbol)
        {
            InstanceTarget = instanceTarget;
            MemberSymbol = memberSymbol;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
