namespace CodeGen.Syntax.Symbols;

internal class EnumTypeSymbol : TypeSymbol
{
    public EnumTypeSymbol(string name)
        : base(name)
    {
    }

    public override string ToString() => $"enum {Name}";
}