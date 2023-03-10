using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler.Shared
{
    internal class TypeSymbol
    {
        public IToken Token { get; private set; }

        public TypeSymbol(IToken token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"{Token.Lexeme}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is TypeSymbol symbol)
            {
                return symbol.Token.Lexeme ==  Token.Lexeme;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Token.Lexeme.GetHashCode();
        }
    }
}
