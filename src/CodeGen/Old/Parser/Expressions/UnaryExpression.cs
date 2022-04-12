namespace CodeGen.Parser.Expressions;

internal class UnaryExpression : Expression
{
    private readonly Expression _right;
    private readonly string _operator;

    public UnaryExpression(Expression right, string @operator)
    {
        _right = right;
        _operator = @operator;
    }
    public override string DebugString() => $"{_operator}{_right.DebugString()}";
}