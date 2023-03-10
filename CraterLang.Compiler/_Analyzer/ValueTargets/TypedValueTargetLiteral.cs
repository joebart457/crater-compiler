using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.ValueTargets
{
    internal class TypedValueTargetLiteral: BaseTypedValueTarget
    {
        public object? Value { get; private set; }

        public TypedValueTargetLiteral(CrateType type, object? value)
            :base(type)
        {
            Value = value;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
