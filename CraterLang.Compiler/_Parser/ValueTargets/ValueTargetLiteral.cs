using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Analyzer.ValueTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Parser.ValueTargets
{
    internal class ValueTargetLiteral : BaseValueTarget
    {
        public object? Value { get; private set; }

        public ValueTargetLiteral(object? value)
        {
            Value = value;
        }

        public override BaseTypedValueTarget Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
