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

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name}");
        Expression.PrettyPrint(indentation + 2);
    }
}