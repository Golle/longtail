using System.Linq;
using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax.Statements;

internal class EnumDeclarationStatement : Statement
{
    public string Name { get; }
    public Expression[] Members { get; }
    public EnumDeclarationStatement(string name, Expression[] members)
    {
        Name = name;
        Members = members;
    }

    public override string ToString() => $"enum {Name} {{\n{string.Join(',', Members.Select(m => m.ToString()))}\n}};";

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Name})", indentation);
        print.Write("Members:", indentation + 2);
        foreach (var member in Members)
        {
            member.PrettyPrint(print, indentation + 4);
        }
    }
}