using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Compiler.Models
{
    internal class CompiledMethod
    {
        public CrateMethod CrateMethod { get; private set; }
        public string GeneratedSource { get; private set; }
        public CompiledMethod(CrateMethod crateMethod, string generatedSource)
        {
            CrateMethod = crateMethod;
            GeneratedSource = generatedSource;
        }
    }
}
