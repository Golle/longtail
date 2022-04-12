namespace CodeGen.Parser.Expressions;

internal class IdentifierExpression : Expression
{
    public string Name { get; }
    public IdentifierExpression(string name)
    {
        Name = name;
    }
    public override string DebugString() => Name;
}