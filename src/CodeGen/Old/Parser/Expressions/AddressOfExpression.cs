namespace CodeGen.Parser.Expressions;

internal class AddressOfExpression : Expression
{
    private readonly Expression _expression;

    public AddressOfExpression(Expression expression)
    {
        _expression = expression;
    }
    public override string DebugString() => $"&{_expression.DebugString()}";
}