using System;

namespace CodeGen.Syntax.Expressions;

internal class ConstExpression : Expression
{
    private readonly Expression _expression;
    public ConstExpression(Expression expression)
    {
        _expression = expression;
    }

    public override string ToString() => $"const {_expression}";

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name}");
        _expression.PrettyPrint(indentation + 2);
    }
}