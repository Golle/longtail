using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;

internal class BoundEnumDeclaration : BoundSyntaxNode
{
    public EnumTypeSymbol Type { get; }
    public BoundExpression[] Members { get; }

    public BoundEnumDeclaration(SyntaxNode node, EnumTypeSymbol type, BoundExpression[] members) : base(node)
    {
        Type = type;
        Members = members;
    }
}