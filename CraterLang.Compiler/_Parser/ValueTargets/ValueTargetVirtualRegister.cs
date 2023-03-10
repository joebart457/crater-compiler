using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;
using CraterLang.Compiler.Shared.Enums;

namespace CraterLang.Compiler._Parser.ValueTargets
{
    internal class ValueTargetVirtualRegister : BaseValueTarget
    {
        public RegisterType Register { get; private set; }

        public ValueTargetVirtualRegister(RegisterType register)
        {
            Register = register;
        }

        public override BaseTypedValueTarget Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
