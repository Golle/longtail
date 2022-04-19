using System;
using System.Linq;

namespace CodeGen.Syntax.Statements;
internal record struct FunctionDeclarationArgument(string Type, string Name);
internal class FunctionDeclarationStatement : Statement
{
    public string ReturnType { get; }
    public string Name { get; }
    public FunctionDeclarationArgument[] Arguments { get; }
    public Statement? Body { get; }

    public FunctionDeclarationStatement(string returnType, string name, FunctionDeclarationArgument[] arguments, Statement? body = null)
    {
        ReturnType = returnType;
        Name = name;
        Arguments = arguments;
        Body = body;
    }

    public override string ToString() => $"{ReturnType} {Name}({string.Join(' ', Arguments.Select(a => $"{a.Type} {a.Name}"))})";

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name} ({ToString()})");
        Body?.PrettyPrint(indentation + 2);
    }
}