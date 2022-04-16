using System.Collections.Generic;

namespace CodeGen.Syntax;

public class SyntaxTree
{
    public static SyntaxTree Parse(string input) =>
        new Parser(input)
            .Parse();


    private readonly SyntaxNode[] _nodes;
    public SyntaxTree(SyntaxNode[] nodes)
    {
        _nodes = nodes;
    }
    
    public IEnumerable<SyntaxNode> GetChildren() => _nodes;
}