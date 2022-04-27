namespace CodeGen.Syntax.Expressions;

internal class ArrayExpression : Expression
{
    public Expression Left { get; }
    public Expression Expression { get; }
    public ArrayExpression(Expression left, Expression expression)
    {
        Left = left;
        Expression = expression;
    }

    public override string ToString() => $"{Left}[{Expression}]";

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name}", indentation);
        Left.PrettyPrint(print, indentation + 2);
        Expression.PrettyPrint(print, indentation + 2);
    }
}