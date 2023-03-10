using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Analyzer.ValueTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Parser.ValueTargets
{
    internal abstract class BaseValueTarget
    {
        public abstract BaseTypedValueTarget Determine(StaticAnalyzer analyzer);
    }
}
