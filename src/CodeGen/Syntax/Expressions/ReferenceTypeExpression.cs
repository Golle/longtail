using System;

namespace CodeGen.Syntax.Expressions;

internal class ReferenceTypeExpression : Expression
{
    public Expression Expression { get; }

    public ReferenceTypeExpression(Expression expression)
    {
        Expression = expression;
    }
    public override string ToString() => $"{Expression} &";

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name} (&)");
        Expression.PrettyPrint(indentation + 2);
    }
}