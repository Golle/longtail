using System.Linq;

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


    public override string ToString()
    {
        var arguments = string.Join(", ", Arguments.Select(a => a));
        return $"{ReturnType} {Name}({arguments})";
    }
}