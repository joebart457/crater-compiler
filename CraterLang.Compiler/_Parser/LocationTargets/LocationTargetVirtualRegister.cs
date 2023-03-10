using CraterLang.Compiler._Analyzer.LocationTargets;
using CraterLang.Compiler._Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;
using Microsoft.Win32;
using CraterLang.Compiler.Shared.Enums;

namespace CraterLang.Compiler._Parser.LocationTargets
{
    internal class LocationTargetVirtualRegister : BaseLocationTarget
    {
        public RegisterType Register { get; private set; }

        public LocationTargetVirtualRegister(RegisterType register)
        {
            Register = register;
        }

        public override BaseTypedLocationTarget Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
