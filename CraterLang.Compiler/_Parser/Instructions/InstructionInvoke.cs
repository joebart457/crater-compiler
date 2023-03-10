using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using TokenizerCore.Interfaces;
using CraterLang.Compiler._Parser.ValueTargets;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal class InstructionInvoke : BaseInstruction
    {
        public IToken MethodName { get; private set; }
        public List<BaseValueTarget> Arguments { get; private set; }
        public InstructionInvoke(IToken methodName, List<BaseValueTarget> arguments)
        {
            MethodName = methodName;
            Arguments = arguments;
        }

        public override BaseTypedInstruction Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
