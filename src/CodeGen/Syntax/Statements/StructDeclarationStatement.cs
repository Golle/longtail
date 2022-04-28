using System;
using System.Text;

namespace CodeGen.Syntax.Statements;

internal class StructDeclarationStatement : Statement
{
    public string Name { get; }
    public VariableDeclarationStatement[] Members { get; }
    public bool ForwardDeclaration { get; }
    public StructDeclarationStatement(string name) : this(name, Array.Empty<VariableDeclarationStatement>(), true){}
    public StructDeclarationStatement(string name, VariableDeclarationStatement[] members, bool forwardDeclaration = false)
    {
        Name = name;
        Members = members;
        ForwardDeclaration = forwardDeclaration;
    }

    public override string ToString()
    {
        if (ForwardDeclaration)
        {
            return $"struct {Name};";
        }
        var builder = new StringBuilder();
        builder.Append("struct ");
        builder.Append(Name);
        builder.AppendLine(" {");
        foreach (var member in Members)
        {
            builder.AppendLine($"  {member.ToString()}");
        }
        builder.AppendLine("}");
        return builder.ToString();
    }

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        if (ForwardDeclaration)
        {
            print.Write($"{GetType().Name} ({Name}, Forward declared)", indentation);
            return;
        }
        print.Write($"{GetType().Name} ({Name})", indentation);
        print.Write("Members:", indentation + 2);
        foreach (var member in Members)
        {
            member.PrettyPrint(print, indentation + 4);
        }
    }
}