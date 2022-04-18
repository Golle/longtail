using System;
using System.Runtime.CompilerServices;

namespace CodeGen.Lexer;
internal record struct StatementDecl(TokenType Type, string Value);
internal static class StatementsTable
{
    private static readonly StatementDecl[] _statements;
    static StatementsTable()
    {
        _statements = new StatementDecl[]
        {
            new(TokenType.If, "if"),
            new(TokenType.Else, "else"),
            new(TokenType.When, "when"),
            new(TokenType.For, "for"),
            new(TokenType.Switch, "switch"),
            new(TokenType.Case, "case"),
            new(TokenType.Default, "default"),
            new(TokenType.Continue, "continue"),
            new(TokenType.Break, "break"),
            new(TokenType.Do, "do"),
            new(TokenType.Return, "return"),
            new(TokenType.Goto, "goto"),
            new(TokenType.Try, "try"),
            new(TokenType.Catch, "catch"),
            new(TokenType.Throw, "throw")
        };
    }

    public static bool TryGetStatement(ReadOnlySpan<char> value, out StatementDecl statement)
    {
        Unsafe.SkipInit(out statement);
        for (var i = 0; i < _statements.Length; ++i)
        {
            if (_statements[i].Value.AsSpan().Equals(value, StringComparison.Ordinal))
            {
                statement = _statements[i];
                return true;
            }
        }
        return false;
    }
}