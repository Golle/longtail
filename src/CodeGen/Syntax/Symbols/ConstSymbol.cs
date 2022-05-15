namespace CodeGen.Syntax.Symbols;

internal class ConstSymbol : Symbol
{
    public Symbol Type { get; }
    public ConstSymbol(Symbol type)
    {
        Type = type;
    }

    public override string ToString() => $"const {Type}";
}