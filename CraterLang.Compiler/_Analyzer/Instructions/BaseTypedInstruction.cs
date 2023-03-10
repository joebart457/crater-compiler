using CraterLang.Compiler._Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.Instructions
{
    internal abstract class BaseTypedInstruction
    {
        public abstract string GenerateSource(StaticCompiler compiler);
    }
}
