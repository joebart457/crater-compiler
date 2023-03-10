using CraterLang.Compiler._Storage.Implementation;
using CraterLang.Compiler._Storage.Interfaces;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.Helpers
{
    internal class MethodProvider
    {
        private IScope<string, CrateMethod> _methods = new Scope<string, CrateMethod>();

        public CrateMethod GetMethod(string name, List<CrateType> parameterTypes)
        {
            var method = _methods.Get(name);
            if (!method.ParameterTypes.SequenceEqual(parameterTypes))
                throw new Exception($"in call {method} expected parameter types ({string.Join(", ", method.ParameterTypes)}) but got ({string.Join(", ", parameterTypes)})");
            return method;
        }

        public void ProvideMethod(CrateMethod method)
        {
            _methods.Define(method.MethodName, method);
        }
    }
}
