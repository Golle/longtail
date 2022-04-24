using System;
using CodeGen.Lexer;

namespace CodeGen.Syntax.Expressions;

internal sealed class LiteralExpression : Expression
{
    public string Value { get; }
    public TokenType Type { get; }
    public LiteralExpression(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Type}:{Value})", indentation);
    }
}