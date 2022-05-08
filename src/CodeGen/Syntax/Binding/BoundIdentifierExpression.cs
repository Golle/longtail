namespace CodeGen.Syntax.Binding;

internal class BoundIdentifierExpression : BoundExpression
{
    public string Value { get; }

    public BoundIdentifierExpression(SyntaxNode node, string value)
        : base(node)
    {
        Value = value;
    }
}