using CodeGen.Syntax.Statements;
using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;

internal class BoundFunctionDeclaration : BoundStatement
{
    public TypeSymbol ReturnType { get; }
    public BoundFunctionDeclaration(Statement statement) 
        : base(statement)
    {
    }
}