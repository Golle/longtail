using CodeGen.Syntax.Statements;
using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;

internal class BoundVariableDeclaration : BoundStatement
{
    public Symbol Type { get; }
    public BoundExpression Variable { get; }
    public BoundVariableDeclaration(VariableDeclarationStatement statement, Symbol type, BoundExpression variable)
        : base(statement)
    {
        Type = type;
        Variable = variable;
    }
}