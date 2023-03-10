using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Parser.Exceptions
{
    public class UnconsumedTokenException : Exception
    {
        public IToken? InvalidToken { get; private set; }
        public UnconsumedTokenException(string message) : base(message) { }
        public UnconsumedTokenException(IToken token, string message) : base($"{message} at {token}")
        {
            InvalidToken = token;
        }
    }
}
