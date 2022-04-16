using System;

namespace CodeGen.Syntax.Expressions;

internal class ParenthesizedExpression : Expression
{
    public Expression Inner { get; }

    public ParenthesizedExpression(Expression inner)
    {
        Inner = inner;
    }

    public override string ToString() => $"({Inner})";

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name}");
        Inner.PrettyPrint(indentation + 2);
    }
}