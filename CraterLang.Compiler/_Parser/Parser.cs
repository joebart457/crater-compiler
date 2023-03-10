using CraterLang.Compiler._Parser.Constants;
using CraterLang.Compiler._Parser.Definitions;
using CraterLang.Compiler._Parser.Exceptions;
using CraterLang.Compiler._Parser.Helpers;
using CraterLang.Compiler._Parser.Instructions;
using CraterLang.Compiler._Parser.LocationTargets;
using CraterLang.Compiler._Parser.ValueTargets;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using CraterLang.Compiler.Shared.Enums;
using System.Globalization;
using TokenizerCore.Interfaces;
using TokenizerCore.Model;

namespace CraterLang.Compiler._Parser
{
    internal class Parser: ParserLite.TokenParser
    {
        private readonly NumberFormatInfo DefaultNumberFormat = new NumberFormatInfo { NegativeSign = "-" };
        private readonly ParserContext _context = new();
        private readonly TokenizerCore.Tokenizer _tokenizer = Tokenizers.Default;
        public IEnumerable<BaseDefinition> ParseFile(string filepath)
        {
            var lines = File.ReadAllText(filepath);
            return ParseTokens(_tokenizer.Tokenize(lines));
        }

        public IEnumerable<BaseDefinition> ParseText(string text)
        {
            return ParseTokens(_tokenizer.Tokenize(text));
        }

        public IEnumerable<BaseDefinition> ParseTokens(IEnumerable<IToken> tokens)
        {
            Initialize(tokens.ToList());
            while (!AtEnd()) yield return ParseDefinition();
            yield break;
        }

        private BaseDefinition ParseDefinition()
        {
            if (AdvanceIfMatch(TokenTypes.Type)) return ParseTypeDefinition();
            if (AdvanceIfMatch(TokenTypes.Fn)) return ParseMethodDefinition();
            throw new Exception($"expect only toplevel statements but got: {Current()}");
        }

        private BaseDefinition ParseTypeDefinition()
        {
            var typeName = Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect type name"));
            var fields = new List<(TypeSymbol, IToken)>();
            if (AdvanceIfMatch(TokenTypes.Error)) {
                fields.Add((new TypeSymbol(new Token(TokenTypes.Word, CTypes.bool_t, -1, -1)), new Token(TokenTypes.Word, "had_error", -1, -1)));
                fields.Add((new TypeSymbol(new Token(TokenTypes.Word, CTypes.string_t, -1, -1)), new Token(TokenTypes.Word, "error_message", -1, -1)));
            }
            Consume(TokenTypes.LCurly, new UnconsumedTokenException(Current(), "expect fields list"));

            do
            {
                fields.Add(ParseParameter());
            } while (!AtEnd() && (AdvanceIfMatch(TokenTypes.Comma) || AdvanceIfMatch(TokenTypes.SemiColon)) && !Match(TokenTypes.RCurly));
            Consume(TokenTypes.RCurly, new UnconsumedTokenException(Current(), "expect enclosing }"));
            return new TypeDefinition(typeName, fields);
        }

        private BaseDefinition ParseMethodDefinition()
        {
            var methodName = Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect method name"));
            
            Consume(TokenTypes.LParen, new UnconsumedTokenException(Current(), "expect parameter list"));
            var parameters = new List<(TypeSymbol, IToken)>();
            if (!AdvanceIfMatch(TokenTypes.RParen))
            {
                do
                {
                    parameters.Add(ParseParameter());
                } while (!AtEnd() && AdvanceIfMatch(TokenTypes.Comma));
                Consume(TokenTypes.RParen, new UnconsumedTokenException(Current(), "expect enclosing )"));
            }

            Consume(TokenTypes.Colon, new UnconsumedTokenException(Current(), "expect : <type>, <err-type>"));
            var returnType = ParseTypeSymbol();
            Consume(TokenTypes.Comma, new UnconsumedTokenException(Current(), "expect <method-name>: <type>, <err-type>"));
            var errorType = ParseTypeSymbol();
            _context.EnterFunctionDefinition();
            var instructions = new List<BaseInstruction>();
            while (!AtEnd() && !Match(TokenTypes.EndFn))
            {
                instructions.Add(ParseInstruction());
            }
            Consume(TokenTypes.EndFn, new UnconsumedTokenException(Current(), "expect end-fn"));
            _context.ExitFunctionDefinition();
            return new MethodDefinition(returnType, errorType, methodName, parameters, instructions);
        }

        #region Helpers

        private TypeSymbol ParseTypeSymbol(string msg = "expect type symbol")
        {
            return new TypeSymbol(Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), msg)));
        }

        private (TypeSymbol typeSymbol, IToken name) ParseParameter()
        {
            var type = ParseTypeSymbol();
            var name = Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect parameter name"));
            return (type, name);
        }

        #endregion

        private BaseInstruction ParseInstruction()
        {
            if (AdvanceIfMatch(TokenTypes.Stor)) return ParseStor();
            if (AdvanceIfMatch(TokenTypes.Mov)) return ParseMov();
            if (AdvanceIfMatch(TokenTypes.If)) return ParseIf();
            if (AdvanceIfMatch(TokenTypes.LoopWhile)) return ParseLoopWhile();
            if (AdvanceIfMatch(TokenTypes.Operate)) return ParseOperate();
            if (AdvanceIfMatch(TokenTypes.SafeInvoke)) return ParseSafeInvoke();
            if (AdvanceIfMatch(TokenTypes.Invoke)) return ParseInvoke();
            if (AdvanceIfMatch(TokenTypes.Get)) return ParseGet();
            if (_context.InsideFunctionDefintion)
            {
                if (AdvanceIfMatch(TokenTypes.Ret)) return ParseRet();
                if (AdvanceIfMatch(TokenTypes.Error)) return ParseError();
            }
            throw new Exception($"unsupported instruction {Current()}");
        }

        private BaseInstruction ParseStor()
        {
            var dest = ParseLocationTarget();
            var src = ParseValueTarget();
            return new InstructionStor(dest, src);
        }

        private BaseInstruction ParseMov()
        {
            var dest = ParseLocationTarget();
            var src = ParseValueTarget();
            return new InstructionMov(dest, src);
        }

        private BaseInstruction ParseIf()
        {
            var valueTarget = ParseValueTarget();
            Consume(TokenTypes.Colon, new UnconsumedTokenException(Current(), "expect : after if"));
            var instructions = new List<BaseInstruction>();
            if (!AtEnd() && !Match(TokenTypes.EndIf))
            {
                instructions.Add(ParseInstruction());
            }
            Consume(TokenTypes.EndIf, new UnconsumedTokenException(Current(), "expect end-if"));

            return new InstructionIf(valueTarget, instructions);
        }

        private BaseInstruction ParseLoopWhile()
        {
            var valueTarget = ParseValueTarget();
            Consume(TokenTypes.Colon, new UnconsumedTokenException(Current(), "expect : after loop-while"));
            var instructions = new List<BaseInstruction>();
            while (!AtEnd() && !Match(TokenTypes.EndLoop))
            {
                instructions.Add(ParseInstruction());
            }
            Consume(TokenTypes.EndLoop, new UnconsumedTokenException(Current(), "expect end-loop"));
            return new InstructionLoopWhile(valueTarget, instructions);
        }

        private BaseInstruction ParseOperate()
        {
            var op = Consume(TokenTypes.Operator, new UnconsumedTokenException(Current(), "expect operator"));
            return new InstructionOperate(op, ParseValueTarget(), ParseValueTarget());
        }

        private BaseInstruction ParseSafeInvoke()
        {
            var method = Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect method to call"));
            var arguments = new List<BaseValueTarget>();
            while (!AtEnd() && !Match(TokenTypes.SemiColon)) {
                arguments.Add(ParseValueTarget());
            }
            Consume(TokenTypes.SemiColon, new UnconsumedTokenException(Current(), "expect ; at end of call"));
            return new InstructionSafeInvoke(method, arguments);
        }

        private BaseInstruction ParseInvoke()
        {
            var method = Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect method to call"));
            var arguments = new List<BaseValueTarget>();
            while (!AtEnd() && !Match(TokenTypes.SemiColon))
            {
                arguments.Add(ParseValueTarget());
            }
            Consume(TokenTypes.SemiColon, new UnconsumedTokenException(Current(), "expect ; at end of call"));
            return new InstructionInvoke(method, arguments);
        }

        private BaseInstruction ParseRet()
        {
            return new InstructionRet(ParseValueTarget());
        }

        private BaseInstruction ParseError()
        {
            return new InstructionError(ParseValueTarget());
        }

        private BaseInstruction ParseGet()
        {
            return new InstructionGet(ParseValueTarget(), Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect member name")));
        }

        private BaseLocationTarget ParseLocationTarget()
        {
            AdvanceIfMatch(TokenTypes.Comma);
            if (AdvanceIfMatch(TokenTypes.Word)) return new LocationTargetVariable(Previous());
            Consume(TokenTypes.RegisterAccessor, new UnconsumedTokenException(Current(), "expect location target"));
            var register = Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect register identifier"));
            return new LocationTargetVirtualRegister(ParseRegisterType(register.Lexeme));
        }

        #region Helpers

        private RegisterType ParseRegisterType(string register)
        {
            if (register == VirtualRegisters.Result || register == VirtualRegisters.Rs) return RegisterType.Result;
            if (register == VirtualRegisters.Err) return RegisterType.Err;
            if (register == VirtualRegisters.OperatorResult || register == VirtualRegisters.Opr) return RegisterType.OperatorResult;
            throw new Exception($"unrecognized register type {register}");
        }

        #endregion

        private BaseValueTarget ParseValueTarget()
        {
            AdvanceIfMatch(TokenTypes.Comma);
            if (AdvanceIfMatch(TokenTypes.RegisterValueAccessor))
            {
                return new ValueTargetVirtualRegister(ParseRegisterType(Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect register specifier")).Lexeme));
            }
            if (AdvanceIfMatch(TokenTypes.Integer))
            {
                return new ValueTargetLiteral(int.Parse(Previous().Lexeme, DefaultNumberFormat));
            }
            if (AdvanceIfMatch(TokenTypes.Double))
            {
                return new ValueTargetLiteral(double.Parse(Previous().Lexeme, DefaultNumberFormat));
            }
            if (AdvanceIfMatch(TokenTypes.Float))
            {
                return new ValueTargetLiteral(float.Parse(Previous().Lexeme, DefaultNumberFormat));
            }
            if (AdvanceIfMatch(TokenTypes.UnsignedInteger))
            {
                return new ValueTargetLiteral(uint.Parse(Previous().Lexeme, DefaultNumberFormat));
            }
            if (AdvanceIfMatch(TokenTypes.False))
            {
                return new ValueTargetLiteral(false);
            }
            if (AdvanceIfMatch(TokenTypes.True))
            {
                return new ValueTargetLiteral(true);
            }
            if (AdvanceIfMatch(TokenTypes.String))
            {
                return new ValueTargetLiteral(Previous().Lexeme);
            }
            if (AdvanceIfMatch(TokenTypes.Char))
            {
                if (!Previous().Lexeme.Any()) return new ValueTargetLiteral('\0');
                return new ValueTargetLiteral(Previous().Lexeme[0]);
            }

            if (AdvanceIfMatch(TokenTypes.SymbolAccessor))
            {
                return new ValueTargetSymbolAccessor(Consume(TokenTypes.Word, new UnconsumedTokenException(Current(), "expect symbol")));
            }

            if (Match(TokenTypes.Subtraction) &&
                (PeekMatch(1, TokenTypes.UnsignedInteger)
                || PeekMatch(1, TokenTypes.Integer)
                || PeekMatch(1, TokenTypes.Float)
                || PeekMatch(1, TokenTypes.Double)))
            {
                Advance();
                if (AdvanceIfMatch(TokenTypes.Integer))
                {
                    return new ValueTargetLiteral(-int.Parse(Previous().Lexeme, DefaultNumberFormat));
                }
                if (AdvanceIfMatch(TokenTypes.Double))
                {
                    return new ValueTargetLiteral(-double.Parse(Previous().Lexeme, DefaultNumberFormat));
                }
                if (AdvanceIfMatch(TokenTypes.Float))
                {
                    return new ValueTargetLiteral(-float.Parse(Previous().Lexeme, DefaultNumberFormat));
                }
                if (AdvanceIfMatch(TokenTypes.UnsignedInteger))
                {
                    return new ValueTargetLiteral(-uint.Parse(Previous().Lexeme, DefaultNumberFormat));
                }
                throw new Exception($"unexpected token while parsing negative {Current()}");
            }

            throw new Exception($"encountered unexpected token {Current()}");
        }
    }
}
