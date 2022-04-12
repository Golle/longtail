namespace CodeGen.Parser.Expressions;

internal class GroupingExpression : Expression
{
    private readonly Expression _expression;
    public GroupingExpression(Expression expression)
    {
        _expression = expression;
    }

    public override string DebugString() => $"({_expression.DebugString()})";
}