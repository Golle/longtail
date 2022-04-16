namespace CodeGen.Syntax;

public class SyntaxTree
{
    public static SyntaxTree Parse(string input) =>
        new Parser(input)
            .Parse();
}