using CodeGen.Parser.Expressions;

namespace CodeGen.Parser.Types;

internal readonly struct EnumMember
{
    public readonly string Name;
    public readonly Expression? Expression;

    public EnumMember(string name)
    {
        Name = name;
        Expression = null;
    }
    public EnumMember(string name, Expression expression)
    {
        Name = name;
        Expression = expression;
    }
}