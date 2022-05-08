using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;



internal class BoundLiteralExpression : BoundExpression
{
    public TypeSymbol Type { get; }
    public string Value { get; } // Should we use a string for all values?
    public BoundLiteralExpression(SyntaxNode node, TypeSymbol type, string value) 
        : base(node)
    {
        Type = type;
        Value = value;
    }
}

internal class BoundBinaryExpression : BoundExpression
{
    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public string Operator { get; }
    public BoundBinaryExpression(SyntaxNode node, BoundExpression left, BoundExpression right, string @operator) 
        : base(node)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }

}

internal abstract class BoundExpression  : BoundSyntaxNode
{
    protected BoundExpression(SyntaxNode node) : base(node)
    {
    }
}

internal abstract class BoundSyntaxNode
{
    // Used for debugging?
    public SyntaxNode Node { get; }
    protected BoundSyntaxNode(SyntaxNode node)
    {
        Node = node;
    }
} 