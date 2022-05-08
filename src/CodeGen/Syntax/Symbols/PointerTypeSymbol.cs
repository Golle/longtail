namespace CodeGen.Syntax.Symbols;

internal class PointerTypeSymbol : Symbol
{
    public Symbol Type { get; }
    public PointerTypeSymbol(Symbol type)
    {
        Type = type;
    }
}