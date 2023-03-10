using CraterLang.Compiler._Compiler;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.ValueTargets
{
    internal class TypedValueTargetSymbolAccessor: BaseTypedValueTarget
    {
        public IToken IdentifierSymbol { get; private set; }

        public TypedValueTargetSymbolAccessor(CrateType type, IToken identifierSymbol)
            :base(type)
        {
            IdentifierSymbol = identifierSymbol;
        }

        public override string GenerateSource(StaticCompiler compiler)
        {
            return compiler.GenerateSource(this);
        }
    }
}
