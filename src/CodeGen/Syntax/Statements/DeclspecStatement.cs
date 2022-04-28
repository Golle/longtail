using CodeGen.Lexer;

namespace CodeGen.Syntax.Statements;

internal class DeclspecStatement : Statement
{
    public TokenType Type { get; }
    public Statement Expression { get; }

    public DeclspecStatement(TokenType type, Statement expression)
    {
        Type = type;
        Expression = expression;
    }
    public override string ToString() => $"__declspec({Type}) {Expression}";

    public override void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{GetType().Name} ({Type})", indentation);
        Expression.PrettyPrint(print, indentation + 2);
    }
}