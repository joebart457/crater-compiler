using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared.Enums
{
    internal enum OperatorType : ushort
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        And,
        Or,
        Equal,
        NotEqual,
        LessThan,
        GreaterThan,
        LessThanEqual,
        GreaterThanEqual,

        // Unary
        Not,
    }
}
