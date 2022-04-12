namespace CodeGen.Parser.Expressions;

internal class DereferenceExpression : Expression
{
    private readonly Expression _expression;

    public DereferenceExpression(Expression expression)
    {
        _expression = expression;
    }

    public override string DebugString() => $"*{_expression.DebugString()}";
}