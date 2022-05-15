using System.Linq;

namespace CodeGen.Syntax.Symbols;

internal class FunctionPointerSymbol : FunctionSymbol
{
    public FunctionPointerSymbol(string name, Symbol returnType, FunctionArgument[] arguments) 
        : base(name, returnType, arguments)
    {
    }

    public override string ToString()
    {
        var arguments = string.Join(", ", Arguments.Select(a => a));
        return $"{ReturnType} {Name}({arguments})";
    }
}