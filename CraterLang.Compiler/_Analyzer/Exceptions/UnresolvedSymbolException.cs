using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.Exceptions
{
    internal class UnresolvedSymbolException: System.Exception
    {
        public TypeSymbol Symbol { get; private set; }

        public UnresolvedSymbolException(TypeSymbol symbol)
            : base($"unresolved symbol {symbol}")
        {
            Symbol = symbol;
        }
    }
}
