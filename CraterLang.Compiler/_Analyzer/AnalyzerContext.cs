using CraterLang.Compiler._Analyzer.Comparers;
using CraterLang.Compiler._Analyzer.Helpers;
using CraterLang.Compiler._Storage.Implementation;
using CraterLang.Compiler._Storage.Interfaces;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using CraterLang.Compiler.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer
{
    internal class AnalyzerContext
    {
        public IStack<IToken, CrateType> Environment { get; private set; }
        public IScope<RegisterType, CrateType> Registers { get; private set; }
        public MethodProvider MethodProvider { get; private set; }
        public TypeProvider TypeProvider { get; private set; }
        private CrateMethod? _enclosingMethod;

        public bool HasEnclosingMethod => _enclosingMethod != null;
        public CrateMethod EnclosingMethod => _enclosingMethod ?? throw new ArgumentNullException(nameof(EnclosingMethod));
        public AnalyzerContext(MethodProvider methodProvider, TypeProvider typeProvider)
        {
            Environment = new Stack<IToken, CrateType>(new TokenComparer());
            Registers = new Scope<RegisterType, CrateType>(null);
            MethodProvider = methodProvider;
            TypeProvider = typeProvider;
        }

        public void SetEnclosingMethod(CrateMethod? method)
        {
            _enclosingMethod = method;
        }

        public CrateType GetRegister(RegisterType register)
        {
            if (!Registers.Exists(register)) throw new Exception($"register {register} is not defined in the current context");
            return Registers.Get(register); 
        }

        public void UpdateRegister(RegisterType register, CrateType value)
        {
            if(Registers.Exists(register)) Registers.Update(register, value);
            else Registers.Define(register, value); 
        }

        public void FlushEnvironment()
        {
            Environment = new Stack<IToken, CrateType>(new TokenComparer());
            _enclosingMethod = null;
        }

        public void FlushRegisters()
        {
            Registers = new Scope<RegisterType, CrateType>(null);
            _enclosingMethod = null;
        }
    }
}
