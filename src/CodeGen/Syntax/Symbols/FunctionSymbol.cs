namespace CodeGen.Syntax.Symbols;

internal class FunctionSymbol : TypeSymbol
{
    public Symbol ReturnType { get; }
    public FunctionArgument[] Arguments { get; }
    public bool Export { get; }
    public FunctionSymbol(string name, Symbol returnType, FunctionArgument[] arguments, bool export = false)
        : base(name)
    {
        ReturnType = returnType;
        Arguments = arguments;
        Export = export;
    }
}