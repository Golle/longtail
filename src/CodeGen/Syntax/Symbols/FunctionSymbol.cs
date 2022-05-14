namespace CodeGen.Syntax.Symbols;

internal class FunctionSymbol : TypeSymbol
{
    public Symbol ReturnType { get; }
    public FunctionArgument[] Arguments { get; }
    public FunctionSymbol(string name, Symbol returnType, FunctionArgument[] arguments)
        : base(name)
    {
        ReturnType = returnType;
        Arguments = arguments;
    }
}