using CodeGen.Syntax.Statements;

namespace CodeGen.Syntax.Binding;

internal class BoundFunctionDeclaration : BoundStatement
{
    public FunctionSymbol FunctionSymbol { get; }
    public BoundFunctionDeclaration(Statement statement, FunctionSymbol functionSymbol) 
        : base(statement)
    {
        FunctionSymbol = functionSymbol;
    }
}