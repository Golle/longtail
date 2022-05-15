namespace CodeGen.Syntax.Symbols;

internal class FunctionArgument
{
    public string Name { get; }
    public Symbol Symbol { get; }

    public FunctionArgument(string name, Symbol symbol)
    {
        Name = name;
        Symbol = symbol;
    }

    public override string ToString() => $"{Symbol} {Name}";
}