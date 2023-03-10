using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.ValueTargets
{
    internal abstract class BaseTypedValueTarget
    {
        public virtual CrateType CrateType { get; }

        public BaseTypedValueTarget(CrateType crateType)
        {
            CrateType = crateType;
        }

        public abstract string GenerateSource(StaticCompiler compiler);
    }
}
