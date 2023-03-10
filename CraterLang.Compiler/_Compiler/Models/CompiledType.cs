using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Compiler.Models
{
    internal class CompiledType
    {
        public CrateType CrateType { get; private set; }
        public string GeneratedSource { get; private set; }
        public CompiledType(CrateType crateType, string generatedSource)
        {
            CrateType = crateType;
            GeneratedSource = generatedSource;
        }
    }
}
