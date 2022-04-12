namespace CodeGen.Parser.Statements;

internal class BlockStatement : Statement
{
    private readonly Statement _statement;

    public BlockStatement(Statement statement)
    {
        _statement = statement;
    }

    public override string DebugString() => $"{{{_statement.DebugString()}}}";
}