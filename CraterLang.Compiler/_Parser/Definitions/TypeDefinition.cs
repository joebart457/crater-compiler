using CraterLang.Compiler._Analyzer;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Parser.Definitions
{
    internal class TypeDefinition: BaseDefinition
    {
        public IToken TypeName { get; private set; }
        public List<(TypeSymbol typeSymbol, IToken name)> Fields { get; private set; }
        public TypeDefinition(IToken typeName, List<(TypeSymbol, IToken)> fields)
        {
            TypeName = typeName;
            Fields = fields;
        }

        public CrateType Determine(StaticAnalyzer analyzer)
        {
            return analyzer.Determine(this);
        }
    }
}
