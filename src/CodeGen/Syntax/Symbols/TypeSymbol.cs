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

    public TypeSymbol(string name, TypeSymbol? parentType)
        : base(name)
    {
        ParentType = parentType;
    }
    public override string ToString() => ParentType != null ? $"{Name} => {ParentType}" : Name;

}


internal class FunctionSymbol : Symbol
{
    public FunctionSymbol(string name) : base(name)
    {
    }
}