namespace CodeGen.Syntax.Symbols;

internal class StructTypeSymbol : TypeSymbol
{
    public StructTypeSymbol(string name)
        : base(name)
    {
    }
    public override string ToString() => ParentType != null ? $"struct {Name} => {ParentType}" : Name;
}