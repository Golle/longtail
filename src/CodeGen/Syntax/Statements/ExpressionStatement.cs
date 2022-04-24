using System;
using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax.Statements;

internal class ExpressionStatement : Statement
{
    public Expression Expression { get; }
    public ExpressionStatement(Expression expression)
    {
        Expression = expression;    
    }
    public override string ToString() => $"{Expression}";

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name}", indentation);
        Expression.PrettyPrint(print, indentation + 2);
    }
}