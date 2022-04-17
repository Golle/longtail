using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CodeGen.Lexer;

internal readonly struct Operator
{
    public readonly string Value;
    public readonly TokenType Type;
    public Operator(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
}

internal class OperatorTable
{
    private static readonly Operator[] _operators;
    static OperatorTable()
    {
        _operators = new Operator[]
            {
                new(TokenType.Colon, ":"),
                new(TokenType.ColonColon, "::"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.Pointer, "->"),
                new(TokenType.Equal, "="),
                new(TokenType.EqualEqual, "=="),
                new(TokenType.Amp, "&"),
                new(TokenType.AmpAmp, "&&"),
                new(TokenType.AmpEqual, "&="),
                new(TokenType.Star, "*"),
                new(TokenType.StarEqual, "*="),
                new(TokenType.Plus, "+"),
                new(TokenType.PlusPlus, "++"),
                new(TokenType.PlusEqual, "+="),
                new(TokenType.Minus, "-"),
                new(TokenType.MinusMinus, "--"),
                new(TokenType.MinusEqual, "-="),
                new(TokenType.Tilde, "~"),
                new(TokenType.Bang, "!"),
                new(TokenType.BangEqual, "!="),
                new(TokenType.Slash, "/"),
                new(TokenType.SlashEqual, "/"),
                new(TokenType.Percent, "%"),
                new(TokenType.PercentEqual, "%="),
                new(TokenType.Less, "<"),
                new(TokenType.LessLess, "<<"),
                new(TokenType.LessEqual, "<="),
                new(TokenType.LessLessEqual, "<<="),
                new(TokenType.Spaceship, "<=>"),
                new(TokenType.Greater, ">"),
                new(TokenType.GreaterGreater, ">>"),
                new(TokenType.GreaterEqual, ">="),
                new(TokenType.GreaterGreaterEqual, ">>="),
                new(TokenType.Caret, "^"),
                new(TokenType.CaretEqual, "^="),
                new(TokenType.Pipe, "|"),
                new(TokenType.PipeEqual, "|="),
                new(TokenType.PipePipe, "||"),
                new(TokenType.Question, "?"),
                new(TokenType.Hash, "#"),
                new(TokenType.HashHash, "##"),
                new(TokenType.HashHat, "#@"),
                new(TokenType.PeriodStar, ".*"),
                new(TokenType.PointerStar, "->*"),
                new(TokenType.Comma, ","),
                new(TokenType.Punctuation, "."),
            }
            .OrderByDescending(v => v.Value.Length)
            .ToArray();
    }

    public static bool TryGetOperator(ReadOnlySpan<char> characters, out Operator op)
    {
        Unsafe.SkipInit(out op);
        for (var i = 0; i < _operators.Length; ++i)
        {
            if (characters.StartsWith(_operators[i].Value, StringComparison.Ordinal))
            {
                op = _operators[i];
                return true;
            }
        }
        return false;
    }
    public static Operator? GetOperator(ReadOnlySpan<char> characters)
    {
        for (var i = 0; i < _operators.Length; ++i)
        {
            if (characters.Equals(_operators[i].Value, StringComparison.Ordinal))
            {
                return _operators[i];
            }
        }
        return null;
    }

}

