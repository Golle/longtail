using CodeGen.Syntax.Statements;

namespace CodeGen.Syntax.Binding;

internal abstract class BoundStatement : BoundSyntaxNode
{
    protected BoundStatement(Statement statement) : base(statement)
    {
    }
}