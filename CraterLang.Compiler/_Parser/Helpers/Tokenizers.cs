using CraterLang.Compiler._Parser.Constants;
using CraterLang.Compiler.Shared.Constants;
using TokenizerCore.Model;

namespace CraterLang.Compiler._Parser.Helpers
{
    internal static class Tokenizers
    {
        public static TokenizerCore.Tokenizer Default
        {
            get
            {
                var settings = TokenizerCore.TokenizerSettings.Default;
                settings.WordIncluded = "_-";
                var rules = new List<TokenizerRule>()
                {
                    new TokenizerRule(TokenTypes.RCurly, "}"),
                    new TokenizerRule(TokenTypes.LCurly, "{"),
                    new TokenizerRule(TokenTypes.RBracket, "]"),
                    new TokenizerRule(TokenTypes.LBracket, "["),
                    new TokenizerRule(TokenTypes.RParen, ")"),
                    new TokenizerRule(TokenTypes.LParen, "("),
                    new TokenizerRule(TokenTypes.LCarat, "<"),
                    new TokenizerRule(TokenTypes.RCarat, ">"),
                    new TokenizerRule(TokenTypes.Dot, "."),
                    new TokenizerRule(TokenTypes.Comma, ","),
                    new TokenizerRule(TokenTypes.LambdaArrow, "=>"),

                    new TokenizerRule(TokenTypes.SemiColon, ";"),
                    new TokenizerRule(TokenTypes.DoubleColon, "::"),

                    new TokenizerRule(TokenTypes.Operator, "-", TokenTypes.Subtraction),
                    new TokenizerRule(TokenTypes.Operator, "+", TokenTypes.Addition),
                    new TokenizerRule(TokenTypes.Operator, "/", TokenTypes.Division),
                    new TokenizerRule(TokenTypes.Operator, "*", TokenTypes.Multiplication),
                    new TokenizerRule(TokenTypes.Operator, "==", TokenTypes.Equal),
                    new TokenizerRule(TokenTypes.Operator, "!=", TokenTypes.NotEqual),
                    new TokenizerRule(TokenTypes.Operator, "<", TokenTypes.LessThan),
                    new TokenizerRule(TokenTypes.Operator, "<=", TokenTypes.LessThanEqual),
                    new TokenizerRule(TokenTypes.Operator, ">", TokenTypes.GreaterThan),
                    new TokenizerRule(TokenTypes.Operator, ">=", TokenTypes.GreaterThanEqual),
                    new TokenizerRule(TokenTypes.Operator, "!", TokenTypes.Not),

                    //Instructions
                    new TokenizerRule(TokenTypes.Stor, "stor"),
                    new TokenizerRule(TokenTypes.Mov, "mov"),
                    new TokenizerRule(TokenTypes.Operate, "operate"),
                    new TokenizerRule(TokenTypes.SafeInvoke, "safe-invoke"),
                    new TokenizerRule(TokenTypes.Invoke, "invoke"),
                    new TokenizerRule(TokenTypes.If, "if"),
                    new TokenizerRule(TokenTypes.EndIf, "end-if"),
                    new TokenizerRule(TokenTypes.LoopWhile, "loop-while"),
                    new TokenizerRule(TokenTypes.EndLoop, "end-loop"),
                    new TokenizerRule(TokenTypes.Ret, "ret"),
                    new TokenizerRule(TokenTypes.Get, "get"),
                    new TokenizerRule(TokenTypes.Error, "error"),

                    new TokenizerRule(TokenTypes.Fn, "fn"),
                    new TokenizerRule(TokenTypes.EndFn, "end-fn"),
                    new TokenizerRule(TokenTypes.Type, "type"),
                    new TokenizerRule(TokenTypes.SymbolAccessor, "$"),
                    new TokenizerRule(TokenTypes.RegisterAccessor, "%"),
                    new TokenizerRule(TokenTypes.RegisterValueAccessor, "%%"),

                    new TokenizerRule(TokenTypes.True, "true"),
                    new TokenizerRule(TokenTypes.False, "false"),
                    new TokenizerRule(TokenTypes.EndOfLineComment, "//"),

                    new TokenizerRule(TokenTypes.Null, "null"),
                    new TokenizerRule(TokenTypes.Var, "var"),
                    new TokenizerRule(TokenTypes.Let, "let"),
                    new TokenizerRule(TokenTypes.This, "this"),
                    new TokenizerRule(TokenTypes.Rec, "rec"),
                    new TokenizerRule(TokenTypes.Gen, "gen"),
                    new TokenizerRule(TokenTypes.Export, "export"),
                    new TokenizerRule(TokenTypes.Inline, "inline"),
                    new TokenizerRule(TokenTypes.Import, "import"),
                    new TokenizerRule(TokenTypes.As, "as"),
                    new TokenizerRule(TokenTypes.Colon, ":"),

                    new TokenizerRule(TokenTypes.Word, "int8", CTypes.int8_t),
                    new TokenizerRule(TokenTypes.Word, "int16", CTypes.int16_t),
                    new TokenizerRule(TokenTypes.Word, "int32", CTypes.int32_t),
                    new TokenizerRule(TokenTypes.Word, "int64", CTypes.int64_t),
                    new TokenizerRule(TokenTypes.Word, "uint8", CTypes.uint8_t),
                    new TokenizerRule(TokenTypes.Word, "uint16", CTypes.uint16_t),
                    new TokenizerRule(TokenTypes.Word, "uint32", CTypes.uint32_t),
                    new TokenizerRule(TokenTypes.Word, "uint64", CTypes.uint64_t),

                    new TokenizerRule(TokenTypes.String, "\"", enclosingLeft: "\"", enclosingRight: "\""),
                    new TokenizerRule(TokenTypes.Char, "'", enclosingLeft: "'", enclosingRight: "'"),
                };
                return new TokenizerCore.Tokenizer(rules, settings);
            }
        }
    }
}
