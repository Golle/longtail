namespace CodeGen.Parser.Expressions;

internal class AssignmentExpression : Expression
{
    private readonly Expression _left;
    private readonly Expression _right;

    public AssignmentExpression(Expression left, Expression right)
    {
        _left = left;
        _right = right;
    }

    public override string DebugString() => $"{_left.DebugString()} = {_right.DebugString()}";
}