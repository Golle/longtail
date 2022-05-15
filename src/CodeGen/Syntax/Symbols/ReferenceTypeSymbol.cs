namespace CodeGen.Syntax.Symbols;

internal class ReferenceTypeSymbol : Symbol
{
    public Symbol Type { get; }
    public ReferenceTypeSymbol(Symbol type)
    {
        Type = type;
    }

    public override string ToString() => $"{Type}&";
}