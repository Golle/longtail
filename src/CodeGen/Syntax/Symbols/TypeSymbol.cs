namespace CodeGen.Syntax.Symbols;

internal class TypeSymbol : Symbol
{
    public string Name { get; }
    public TypeSymbol(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}