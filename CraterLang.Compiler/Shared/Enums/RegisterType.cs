using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared.Enums
{
    internal enum RegisterType: ushort
    {
        Err,
        Result,
        OperatorResult,
    }
}
