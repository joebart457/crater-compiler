using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.LocationTargets
{
    internal class TypedLocationTargetVariable: BaseTypedLocationTarget
    {
        public IToken VariableName { get; private set; }

        public TypedLocationTargetVariable(CrateType type, IToken variableName)
            :base(type)
        {
            VariableName = variableName;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
