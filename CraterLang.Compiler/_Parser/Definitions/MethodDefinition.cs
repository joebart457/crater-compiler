using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler._Parser.Instructions;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Parser.Definitions
{
    internal class MethodDefinition: BaseDefinition
    {
        public TypeSymbol ReturnType { get; private set; }
        public TypeSymbol ErrorType { get; private set; }
        public IToken MethodName { get; private set; }
        public List<(TypeSymbol typeSymbol, IToken name)> Parameters { get; private set; }
        public List<BaseInstruction> Instructions { get; private set; }
        public MethodDefinition(TypeSymbol returnType, TypeSymbol errorType, IToken methodName, List<(TypeSymbol, IToken)> parameters, List<BaseInstruction> instructions)
        {
            ReturnType = returnType;
            ErrorType = errorType;
            MethodName = methodName;
            Parameters = parameters;
            Instructions = instructions;
        }

        public CrateMethod Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
