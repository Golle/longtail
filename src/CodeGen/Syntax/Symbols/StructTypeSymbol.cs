using System.Text;
using CodeGen.Syntax.Binding;

namespace CodeGen.Syntax.Symbols;

internal sealed class ArrayStructMember : StructMember
{
    public BoundExpression Initializer { get; }
    public ArrayStructMember(string name, Symbol type, BoundExpression initializer) 
        : base(name, type)
    {
        Initializer = initializer;
    }

    public override string ToString() => $"{Type} {Name}[{Initializer}]";
}

internal class StructMember
{
    public string Name { get; }
    public Symbol Type { get; }

    public StructMember(string name, Symbol type)
    {
        Name = name;
        Type = type;
    }

    public override string ToString() => $"{Type} {Name}";
}

internal sealed class StructTypeSymbol : TypeSymbol
{
    public StructMember[] Members { get; }
    public StructTypeSymbol(string name, StructMember[] members)
        : base(name)
    {
        Members = members;
    }
    public override string ToString() => $"struct {Name}";
}