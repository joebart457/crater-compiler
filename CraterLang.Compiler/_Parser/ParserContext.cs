using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Parser
{
    internal class ParserContext
    {
        public bool InsideFunctionDefintion { get; private set; }

        public void EnterFunctionDefinition()
        {
            if (InsideFunctionDefintion) throw new Exception("nested functions not allowed");
            InsideFunctionDefintion = true;
        }

        public void ExitFunctionDefinition()
        {
            if (!InsideFunctionDefintion) throw new Exception("unable to exit, no function entered");
            InsideFunctionDefintion = false;
        }
    }
}
