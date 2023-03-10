using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.LocationTargets
{
    internal class TypedLocationTargetVirtualRegister: BaseTypedLocationTarget
    {
        public RegisterType Register { get; private set; }

        public TypedLocationTargetVirtualRegister(CrateType type, RegisterType register)
            :base(type)
        {
            Register = register;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
