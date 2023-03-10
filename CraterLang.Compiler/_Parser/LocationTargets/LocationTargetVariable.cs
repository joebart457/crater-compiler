using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Analyzer.LocationTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Parser.LocationTargets
{
    internal class LocationTargetVariable : BaseLocationTarget
    {
        public IToken VariableName { get; private set; }

        public LocationTargetVariable(IToken variableName)
        {
            VariableName = variableName;
        }

        public override BaseTypedLocationTarget Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
