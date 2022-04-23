using System;
using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax.Statements;

internal class VariableDeclarationStatement : Statement
{
    public Expression Type { get; } //Note(Jens): Replace this with types when we support that (or that could be a different pass?)
    public string Identifier { get; }
    public Expression? AssignmentExpression { get; }
    public VariableDeclarationStatement(Expression type, string identifier, Expression? assignmentExpression)
    {
        Type = type;
        Identifier = identifier;
        AssignmentExpression = assignmentExpression;
    }


    public override string ToString()
    {
        if (AssignmentExpression != null)
        {
            return $"{Type} {Identifier} = {AssignmentExpression};";
        }
        return $"{Type} {Identifier};";
    }

    public override void PrettyPrint(int indentation = 0)
    {
        Console.WriteLine($"{new string(' ', indentation)}{GetType().Name} ({Identifier})");
        Type.PrettyPrint(indentation + 2);
        AssignmentExpression?.PrettyPrint(indentation + 2);
    }
}