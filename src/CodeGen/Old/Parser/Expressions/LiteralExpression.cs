namespace CodeGen.Parser.Expressions;

internal class LiteralExpression : Expression
{
    private readonly string _value;
    public LiteralExpression(string value)
    {
        _value = value;
    }
    public override string DebugString() => _value;
}