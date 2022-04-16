using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodeGen.Lexer;

namespace CodeGen.Syntax;

internal class Parser
{
    public readonly Token[] _tokens;

    public Parser(string input)
    {
        _tokens =  new Tokenizer().Tokenize(input).ToArray();
    }


    public SyntaxTree Parse()
    {
        // add implemnetaiton for parsing exrpressions

        return new SyntaxTree();
    }
}


[DebuggerDisplay("{ToString()}")]
public abstract class ExpressionNode
{
    
}
public abstract class SyntaxNode
{

}