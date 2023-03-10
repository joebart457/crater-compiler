using CraterLang.Compiler._Analyzer.Exceptions;
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
    internal class TypeProvider
    {
        private IScope<string, CrateType> _types = new Scope<string, CrateType>();

        public CrateType ResolveToType(TypeSymbol symbol)
        {
            if (!_types.Exists(symbol.Token.Lexeme)) throw new UnresolvedSymbolException(symbol);
            return _types.Get(symbol.Token.Lexeme);
        }

        public void ProvideType(CrateType type)
        {
            _types.Define(type.CType, type);
        }

        public bool CanResolve(TypeSymbol type)
        {
            return _types.Exists(type.Token.Lexeme);
        }
    }
}
