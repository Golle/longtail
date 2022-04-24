using System;
using System.Text;
using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax.Statements;

internal record struct StructMember(Expression Type, string Name);
internal class StructDeclarationStatement : Statement
{
    public string Name { get; }
    public StructMember[] Members { get; }
    public bool ForwardDeclaration { get; }
    public StructDeclarationStatement(string name) : this(name, Array.Empty<StructMember>(), true){}
    public StructDeclarationStatement(string name, StructMember[] members, bool forwardDeclaration = false)
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
        foreach (var (type, name) in Members)
        {
            builder.AppendLine($"  {type} {name}");
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
        foreach (var (type, name) in Members)
        {
            print.Write($"{name}:", indentation + 4);
            type.PrettyPrint(print, indentation + 6);
        }
    }
}