using CraterLang.Compiler._Analyzer.Instructions;
using CraterLang.Compiler._Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Parser.Instructions
{
    internal abstract class BaseInstruction
    {
        public abstract BaseTypedInstruction Determine(StaticAnalyzer analyzer);
    }
}
