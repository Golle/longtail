using System;

namespace CodeGen.Syntax.Expressions;

internal class StructExpression : Expression
{
    public Expression Expression { get; }
    public StructExpression(Expression expression)
    {
        Expression = expression;
    }

    public override string ToString() => $"struct {Expression}";
    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name}");
        Expression.PrettyPrint(indentation + 2);
    }
}