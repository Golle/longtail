using System;
using System.Linq;
using CodeGen.Lexer;

namespace CodeGen.Syntax.Expressions;

internal class BuiltInTypeExpression : Expression
{
    public TokenType[] Types { get; }
    public BuiltInTypeExpression(ReadOnlySpan<TokenType> types)
    {
        Types = types.ToArray();
    }

    public override string ToString() => string.Join(' ', Types.Select(t => t));

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name} ({ToString()})");
    }
}