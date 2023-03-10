using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared.Enums;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.ValueTargets
{
    internal class TypedValueTargetVirtualRegister : BaseTypedValueTarget
    {
        public RegisterType Register { get; private set; }

        public TypedValueTargetVirtualRegister(CrateType type, RegisterType register)
            : base(type)
        {
            Register = register;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
