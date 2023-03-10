using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.Constants
{
    internal static class CNativeMethods
    { 
        public static CrateMethod PrintLn => new CrateMethod(FullCTypes.int32_t, FullCTypes.error_result_t, "println", new List<CrateMethodParameter>() { new(FullCTypes.string_t, "message") } );
    }
}
