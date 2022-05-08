namespace CodeGen.Syntax.Symbols;

internal class TypeSymbol : Symbol
{
    public TypeSymbol? ParentType { get; }
    public string Name { get; }
    public TypeSymbol(string name, TypeSymbol? parentType = null)
    {
        Name = name;
        ParentType = parentType;
    }

    public override string ToString() => ParentType != null ? $"{Name} => {ParentType}" : Name;
}