namespace CodeGen.Syntax.Expressions;

internal class PointerTypeExpression : Expression
{
    public Expression Expression { get; }
    public PointerTypeExpression(Expression expression)
    {
        Expression = expression;
    }

    public override string ToString() => $"{Expression} *";

    public override void PrettyPrint(IPrettyPrint print, int indentation)
    {
        print.Write($"{GetType().Name} (*)", indentation);
        Expression.PrettyPrint(print, indentation + 2);
    }
}