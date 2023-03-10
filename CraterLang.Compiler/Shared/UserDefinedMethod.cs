using CraterLang.Compiler._Analyzer.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared
{
    internal class UserDefinedMethod : CrateMethod
    {
        public List<BaseTypedInstruction> Instructions { get; private set; }
        public UserDefinedMethod(CrateType returnType, CrateType errorType, string methodName, List<CrateMethodParameter> parameters, List<BaseTypedInstruction> instructions)
            : base(returnType, errorType, methodName, parameters)
        {
            Instructions = instructions;
        }
    }
}
