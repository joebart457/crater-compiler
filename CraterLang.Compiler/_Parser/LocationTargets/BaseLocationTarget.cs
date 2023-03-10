using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Analyzer.LocationTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Parser.LocationTargets
{
    internal abstract class BaseLocationTarget
    {
        public abstract BaseTypedLocationTarget Determine(StaticAnalyzer analyzer);
    }
}
