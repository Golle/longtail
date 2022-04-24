using System;
using System.Linq;
using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax.Statements;

internal record struct FunctionDeclarationArgument(Expression Type, string Name);

internal class FunctionDeclarationStatement : Statement
{
    public Expression ReturnType { get; }
    public string Name { get; }
    public FunctionDeclarationArgument[] Arguments { get; }
    public Statement? Body { get; }

    public FunctionDeclarationStatement(Expression returnType, string name, FunctionDeclarationArgument[] arguments, Statement? body = null)
    {
        ReturnType = returnType;
        Name = name;
        Arguments = arguments;
        Body = body;
    }

    public override string ToString() => $"{ReturnType} {Name}({string.Join(' ', Arguments.Select(a => $"{a.Type} {a.Name}"))})";

    public override void PrettyPrint(int indentation = 0)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Print(GetType().Name);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Print("Return type:", 2);
        Console.ResetColor();
        ReturnType.PrettyPrint(indentation + 4);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Print("Arguments:", 2);
        Console.ResetColor();
        foreach (var argument in Arguments)
        {
            Print($"{argument.Name}:", 4);
            argument.Type.PrettyPrint(6);
        }

        if (Body != null)
        {
            Print("Body:", 2);
            Body?.PrettyPrint(indentation + 4);
        }
        void Print(string message, int extra = 0) => Console.WriteLine($"{new string(' ', indentation + extra)}{message}");
    }
}