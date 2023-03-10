using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared
{
    internal class CrateMethod
    {
        public CrateType ReturnType { get; private set; }
        public CrateType ErrorType { get; private set; }
        public string MethodName { get; private set; }
        public List<CrateMethodParameter> Parameters { get; private set; }
        public CrateMethod(CrateType returnType, CrateType errorType, string methodName, List<CrateMethodParameter> parameters)
        {
            ReturnType = returnType;
            ErrorType = errorType;
            MethodName = methodName;
            Parameters = parameters;
        }

        public List<CrateType> ParameterTypes => Parameters.Select(p => p.CrateType).ToList();
    }
}
