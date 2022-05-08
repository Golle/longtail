using CodeGen.Syntax.Statements;
using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;

internal class BoundStructDeclaration : BoundStatement
{
    public StructTypeSymbol Type { get; }
    public BoundStructDeclaration(Statement statement, StructTypeSymbol type)
        : base(statement)
    {
        Type = type;
    }
}