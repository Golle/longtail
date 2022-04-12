using CodeGen.Parser.Expressions;
using CodeGen.Parser.Types;

namespace CodeGen.Parser.Statements;

internal class DeclarationStatement : Statement
{
    private readonly TypeDeclaration _type;
    private readonly Expression _variable;
    public DeclarationStatement(TypeDeclaration type, Expression variable)
    {
        _type = type;
        _variable = variable;
    }

    public override string DebugString() => $"{_type} {_variable.DebugString()}";
}