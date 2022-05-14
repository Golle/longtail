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

    public override void PrettyPrint(IPrettyPrint print, int indentation)
    {
        print.Write($"{GetType().Name} ({Operator})", indentation);
        Expression.PrettyPrint(print, indentation + 2);
    }
}