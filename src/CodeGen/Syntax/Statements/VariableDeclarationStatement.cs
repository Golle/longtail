using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax.Statements;

internal class VariableDeclarationStatement : Statement
{
    public Expression Type { get; } //Note(Jens): Replace this with types when we support that (or that could be a different pass?)
    public Expression Variable { get; }
    public VariableDeclarationStatement(Expression type, Expression variable)
    {
        Type = type;
        Variable = variable;
    }

    public override string ToString() => $"{Type} {Variable};";

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Variable.GetType().Name})", indentation);
        Type.PrettyPrint(print, indentation + 2);
        Variable.PrettyPrint(print, indentation + 2);
    }
}