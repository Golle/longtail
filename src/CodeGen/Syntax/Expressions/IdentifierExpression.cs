using System;

namespace CodeGen.Syntax.Expressions;

internal class IdentifierExpression : Expression
{
    public string Value { get; }

    public IdentifierExpression(string value)
    {
        Value = value;
    }

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name} ({Value})");
    }
}