namespace CodeGen.Parser.Expressions;

internal class AccessorExpression : Expression
{
    private readonly Expression _left;
    private readonly Expression _right;
    private readonly string _accessor;
    public AccessorExpression(Expression left, Expression right, string accessor)
    {
        _left = left;
        _right = right;
        _accessor = accessor;
    }

    public override string DebugString() => $"{_left.DebugString()}{_accessor}{_right.DebugString()}";
}