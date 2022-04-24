using System;

namespace CodeGen.Syntax.Expressions;

internal class AssigmentExpression : Expression
{
    public Expression Left { get; }
    public Expression Right { get; }
    public string Operator { get; }

    public AssigmentExpression(Expression left, Expression right, string op)
    {
        Left = left;
        Right = right;
        Operator = op;
    }

    public override string ToString() => $"{Left} {Operator} {Right}";

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Operator})", indentation);
        Left.PrettyPrint(print, indentation + 2);
        Right.PrettyPrint(print, indentation + 2);
    }
}