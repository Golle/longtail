namespace CodeGen.Syntax.Symbols;

internal class FunctionPointerSymbol : FunctionSymbol
{
    public FunctionPointerSymbol(string name, Symbol returnType, FunctionArgument[] arguments) 
        : base(name, returnType, arguments)
    {
    }
}