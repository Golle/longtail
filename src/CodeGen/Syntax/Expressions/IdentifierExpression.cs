namespace CodeGen.Syntax.Expressions;

internal class IdentifierExpression : Expression
{
    public string Value { get; }

    public IdentifierExpression(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Value})", indentation);
    }
}