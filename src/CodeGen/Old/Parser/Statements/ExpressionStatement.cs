using CodeGen.Parser.Expressions;

namespace CodeGen.Parser.Statements;

internal class ExpressionStatement : Statement
{
    private readonly Expression _expression;
    public ExpressionStatement(Expression expression)
    {
        _expression = expression;
    }
    public override string DebugString() => _expression.DebugString();
}