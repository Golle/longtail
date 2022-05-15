namespace CodeGen.Syntax.Symbols;

internal class PrimitiveTypeSymbol : TypeSymbol
{
    public int Size { get; }
    public bool Unsigned { get; }
    public PrimitiveTypeSymbol(string name, int size, bool unsigned = false) 
        : base(name)
    {
        Size = size;
        Unsigned = unsigned;
    }

    public override string ToString() => Name;
}