namespace CodeGen.Syntax.Statements;

internal class TypedefStatement : Statement
{
    public string Name { get; }
    public Statement Definition { get; }

    public TypedefStatement(string name, Statement definition)
    {
        Name = name;
        Definition = definition;
    }

    public override string ToString() => $"typedef {Definition} {Name}";
    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Name})", indentation);
        Definition.PrettyPrint(print, indentation + 2);
    }
}