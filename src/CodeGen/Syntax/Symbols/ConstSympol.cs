namespace CodeGen.Syntax.Symbols;

internal class ConstSympol : Symbol
{
    public Symbol Type { get; }
    public ConstSympol(Symbol type)
    {
        Type = type;
    }
}