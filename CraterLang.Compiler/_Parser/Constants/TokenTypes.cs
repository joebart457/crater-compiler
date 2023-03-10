using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Parser.Constants
{
    public static class TokenTypes
    {
        public const string SymbolAccessor = "SymbolAccessor";
        public const string RegisterAccessor = "RegisterAccessor";
        public const string RegisterValueAccessor = "RegisterValueAccessor";


        // Instructions
        public const string SafeInvoke = "SafeInvoke";
        public const string Invoke = "Invoke";
        public const string Stor = "Stor";
        public const string Mov = "Mov";
        public const string Get = "Get";
        public const string LoopWhile = "LoopWhile";
        public const string EndLoop = "EndLoop";
        public const string If = "If";
        public const string EndIf = "EndIf";
        public const string Operate = "Operate";
        public const string Ret = "Ret";
        public const string Error = "Error";
        public const string Operator = "Operator";

        // Definitions
        public const string Type = "Type";
        public const string Fn = "Fn";
        public const string EndFn = "EndFn";


        public const string LCurly = "LCurly";
        public const string RCurly = "RCurly";
        public const string LBracket = "LBracket";
        public const string RBracket = "RBracket";
        public const string LParen = "LParen";
        public const string RParen = "RParen";
        public const string LCarat = "LCarat";
        public const string RCarat = "RCarat";
        public const string Dot = "Dot";
        public const string Comma = "Comma";
        public const string LambdaArrow = "LambdaArrow";

        public const string SemiColon = "SemiColon";
        public const string DoubleColon = "DoubleColon";

        //Special Types
        public const string Char = "Char";

        //Keywords
        public const string Var = "Var";
        public const string Let = "Let";
        public const string This = "This";
        public const string Rec = "Rec";
        public const string Gen = "Gen";
        public const string Export = "Export";
        public const string Inline = "Inline";
        public const string Import = "Import";
        public const string As = "As";

        // Value Types
        public const string False = "False";
        public const string True = "True";
        public const string Null = "null";

        // Built-in Types
        public const string Space = "Space";
        public const string Tab = "Tab";
        public const string CarriageReturn = "CarriageReturn";
        public const string LineFeed = "LineFeed";

        public const string Word = "TTWord";
        public const string String = "TTString";
        public const string Integer = "TTInteger";
        public const string UnsignedInteger = "TTUnsignedInteger";
        public const string Double = "TTDouble";
        public const string Float = "TTFloat";

        public const string EndOfLineComment = "EndOfLineComment";
        public const string MultiLineComment = "MultiLineComment";

        public const string EndOfFile = "EndOfFile";

        public const string Assignment = "Assignment";
        public const string Colon = "Colon";
        public const string DoubleQuestionMark = "DoubleQuestionMark";
        public const string DoubleDot = "DoubleDot";
        public const string DoubleRBracket = "DoubleRBracket";
        public const string DoubleLBracket = "DoubleLBracket";

        public const string Equal = "Equal";
        public new const string Equals = "Equals";
        public const string NotEqual = "NotEqual";
        public const string GreaterThan = "GreaterThan";
        public const string GreaterThanEqual = "GreaterThanEqual";
        public const string LessThan = "LessThan";
        public const string LessThanEqual = "LessThanEqual";
        public const string And = "And";
        public const string Or = "Or";
        public const string Not = "Not";
        public const string Subtraction = "Subtraction";
        public const string Addition = "Addition";
        public const string Multiplication = "Multiplication";
        public const string Division = "Division";

        public const string Else = "Else";
        public const string While = "While";
        public const string Return = "Return";
        public const string Continue = "Continue";
        public const string Break = "Break";
    }
}
