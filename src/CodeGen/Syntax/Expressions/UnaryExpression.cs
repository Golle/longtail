using System;

namespace CodeGen.Syntax.Expressions;

public class UnaryExpression : Expression
{
    public Expression Expression { get; }
    public string Operator { get; }
    public UnaryExpression(string  op, Expression expression)
    {
        Expression = expression;
        Operator = op;
    }

    public override string ToString() => $"{Operator}{Expression}";

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name} ({Operator})");
        Expression.PrettyPrint(indentation + 2);
    }
}