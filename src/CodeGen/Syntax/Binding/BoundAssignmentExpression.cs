namespace CodeGen.Syntax.Binding;

internal class BoundAssignmentExpression : BoundExpression
{
    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public string Operator { get; }

    public BoundAssignmentExpression(SyntaxNode node, BoundExpression left, BoundExpression right, string @operator)
        : base(node)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }
}