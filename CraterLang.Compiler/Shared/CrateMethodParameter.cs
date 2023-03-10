using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared
{
    internal class CrateMethodParameter
    {
        public CrateType CrateType { get; private set; }
        public string ParameterName { get; private set; }
        public CrateMethodParameter(CrateType crateType, string parameterName)
        {
            CrateType = crateType;
            ParameterName = parameterName;
        }
    }
}
