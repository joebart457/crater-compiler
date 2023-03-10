using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared
{
    internal class UserDefinedType : CrateType
    {
        public UserDefinedType(string cType) : base(cType)
        {
        }
    }
}
