using System.Diagnostics.CodeAnalysis;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.Comparers
{
    internal class TokenComparer : EqualityComparer<IToken>
    {
        public override bool Equals(IToken? x, IToken? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            return x.Lexeme == y.Lexeme;
        }

        public override int GetHashCode([DisallowNull] IToken obj)
        {
            return obj.Lexeme.GetHashCode();
        }
    }
}
