using System;
using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax.Statements;

internal class FunctionDeclarationStatement : Statement
{
    public Expression ReturnType { get; }
    public string Name { get; }
    public VariableDeclarationStatement[] Arguments { get; }
    public TokenType[] Modifiers { get; }
    public Statement? Body { get; }

    public FunctionDeclarationStatement(Expression returnType, string name, VariableDeclarationStatement[] arguments, Statement? body = null)
    : this(returnType, name, arguments, Array.Empty<TokenType>(), body)
    {
    }

    public FunctionDeclarationStatement(Expression returnType, string name, VariableDeclarationStatement[] arguments, TokenType[] modifiers, Statement? body = null)
    {
        ReturnType = returnType;
        Name = name;
        Arguments = arguments;
        Modifiers = modifiers;
        Body = body;
    }

    public override string ToString() => $"{string.Join(' ', Modifiers)} {ReturnType} {Name}({string.Join(' ', Arguments.Select(a => $"{a.Type}"))})";

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Name})", indentation);
        if (Modifiers.Length > 0)
        {
            print.Write("Modifiers:", indentation + 2);
            foreach (var tokenType in Modifiers)
            {
                print.Write(tokenType.ToString(), indentation + 4);
            }
        }
        print.Write("Return type:", indentation + 2);
        ReturnType.PrettyPrint(print, indentation + 4);
        print.Write("Arguments:", indentation + 2);
        foreach (var argument in Arguments)
        {
            argument.Type.PrettyPrint(print, indentation + 4);
        }

        if (Body != null)
        {
            print.Write("Body:", indentation + 2);
            Body?.PrettyPrint(print, indentation + 4);
        }
    }
}