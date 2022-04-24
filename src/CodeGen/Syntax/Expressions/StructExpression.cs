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
    public override void PrettyPrint(IPrettyPrint print, int indentation)
    {
        print.Write($"{GetType().Name}", indentation);
        Expression.PrettyPrint(print, indentation + 2);
    }
}