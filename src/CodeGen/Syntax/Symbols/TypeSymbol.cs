namespace CodeGen.Syntax.Symbols;


internal abstract class Symbol
{
    public string Name { get; }
    protected Symbol(string name)
    {
        Name = name;
    }
}


internal class TypeSymbol : Symbol
{
    public TypeSymbol? ParentType { get; }

    public TypeSymbol(string name, TypeSymbol? parentType = null)
        : base(name)
    {
        ParentType = parentType;
    }
    public override string ToString() => ParentType != null ? $"{Name} => {ParentType}" : Name;

}

internal class PrimitiveTypeSymbol : TypeSymbol
{
    public int Size { get; }
    public bool Unsigned { get; }
    public PrimitiveTypeSymbol(string name, int size, bool unsigned = false) 
        : base(name, null)
    {
        Size = size;
        Unsigned = unsigned;
    }
}

internal class FunctionSymbol : Symbol
{
    public FunctionSymbol(string name) : base(name)
    {
    }
}