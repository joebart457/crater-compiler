using CraterLang.Compiler._Analyzer.ValueTargets;
using CraterLang.Compiler._Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Parser.ValueTargets
{
    internal class ValueTargetSymbolAccessor : BaseValueTarget
    {
        public IToken IdentifierSymbol { get; private set; }

        public ValueTargetSymbolAccessor(IToken identifierSymbol)
        {
            IdentifierSymbol = identifierSymbol;
        }

        public override BaseTypedValueTarget Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
