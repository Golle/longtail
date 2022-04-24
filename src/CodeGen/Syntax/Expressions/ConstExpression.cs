namespace CodeGen.Syntax.Expressions;

internal class ConstExpression : Expression
{
    public Expression Expression { get; }
    public ConstExpression(Expression expression)
    {
        Expression = expression;
    }

    public override string ToString() => $"const {Expression}";
    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name}", indentation);
        Expression.PrettyPrint(print, indentation + 2);
    }
}