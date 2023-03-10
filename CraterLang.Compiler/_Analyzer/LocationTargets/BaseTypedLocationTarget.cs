using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.LocationTargets
{
    internal abstract class BaseTypedLocationTarget
    {
        public virtual CrateType CrateType { get; }

        public BaseTypedLocationTarget(CrateType crateType)
        {
            CrateType = crateType;
        }

        public abstract string GenerateSource(StaticCompiler compiler);

    }
}
