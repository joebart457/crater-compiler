using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraterLang.Compiler._Parser.ValueTargets;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal class InstructionError : BaseInstruction
    {
        public BaseValueTarget ErrorToReturn { get; private set; }

        public InstructionError(BaseValueTarget errorToReturn)
        {
            ErrorToReturn = errorToReturn;
        }

        public override BaseTypedInstruction Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
