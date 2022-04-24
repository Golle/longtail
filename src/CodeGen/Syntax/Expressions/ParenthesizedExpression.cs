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

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name}", indentation);
        Inner.PrettyPrint(print, indentation + 2);
    }
}