namespace CodeGen.Parser.Expressions;

internal class ArrayExpression : Expression
{
    private readonly Expression _left;
    private readonly Expression _expression;
    public ArrayExpression(Expression left, Expression expression)
    {
        _left = left;
        _expression = expression;
    }
    public override string DebugString() => $"{_left.DebugString()}[{_expression.DebugString()}]";
}