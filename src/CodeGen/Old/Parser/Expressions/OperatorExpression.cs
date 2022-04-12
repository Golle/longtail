namespace CodeGen.Parser.Expressions;

internal class OperatorExpression : Expression
{
    private readonly Expression _left;
    private readonly Expression _right;
    private readonly string _operator;
    public OperatorExpression(Expression left, Expression right, string @operator)
    {
        _left = left;
        _right = right;
        _operator = @operator;
    }

    public override string DebugString() => $"{_left.DebugString()} {_operator} {_right.DebugString()}";
}