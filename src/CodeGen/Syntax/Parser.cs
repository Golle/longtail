using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax.Expressions;

namespace CodeGen.Syntax;

internal class Parser
{
    private static readonly Token InvalidToken = new() { Type = TokenType.Invalid };
    private readonly Token[] _tokens;
    private int _position;


    private Token Current => Peek(0);
    //private Token ConsumeToken()
    //{
    //    return _tokens[_position++];
    //}

    private Token Peek(int steps)
    {
        var position = _position + steps;
        return position < _tokens.Length ? _tokens[position] : InvalidToken;
    }

    private Token Consume()
    {
        var current = Current;
        _position++;
        return current;
    }

    public Parser(string input)
    {
        _tokens = new Tokenizer().Tokenize(input).ToArray();
    }

    public SyntaxTree Parse()
    {
        List<SyntaxNode> nodes = new();

        var parsing = true;
        while (parsing)
        {
            var token = Current;
            
            switch (token.Type)
            {

                case TokenType.Struct:
                    nodes.Add(ParseStruct());
                    break;

                case TokenType.EndOfFile:
                case TokenType.Invalid:
                    parsing = false;
                    break;
                default:
                    nodes.Add(ParseExpression());
                    break;
            }
            _position++;
        }
        return new SyntaxTree(nodes.ToArray());
    }

    private Expression ParseExpression()
    {
        var binary = ParseBinaryExpression();

        return binary;

    }

    private Expression ParseBinaryExpression()
    {
        var primary = ParsePrimaryExpression();
        if (Current.Type is TokenType.Operator)
        {
            var opToken = Current;
            _position++;
            var right = ParseExpression();
            return new BinaryExpression(primary, right, opToken.Value);
        }

        if (Current.Type is TokenType.CompoundOperator)
        {
            var opToken = Current;
            if (Current.Value is "++" or "--")
            {
                // special case ?
            }

            _position++;
            var right = ParseExpression();
            return new BinaryExpression(primary, right, opToken.Value);
        }

        return primary;
    }

    private Expression ParsePrimaryExpression()
    {
        var current = Current;

        if (current.Type is TokenType.Boolean or TokenType.String or TokenType.Number or TokenType.Character)
        {
            _position++;
            return new LiteralExpression(current.Type, current.Value);
        }
        
        if (current.Type == TokenType.Identifier)
        {
            _position++;
            return new IdentifierExpression(current.Value);
        }

        if (current.Type == TokenType.LeftParenthesis)
        {
            _position++;
            var inside = ParseExpression();
            if (Current.Type != TokenType.RightParenthesis)
            {
                throw new ParserException($"Expected {TokenType.RightParenthesis} but found {TokenType.LeftParenthesis}");
            }
            _position++;
            return new ParenthesizedExpression(inside);
        }

        throw new ParserException($"Primary expression for token type {current.Type} has not been implemented. FIX IT!");
    }

    private SyntaxNode ParseStruct()
    {
        var next = Peek(1);
        if (next.Type == TokenType.Identifier)
        {
            if (Peek(2).Type == TokenType.LeftCurlyBracer)
            {
                _position += 2;
                
                var members = ParseMembers();
                // struct definition
                if (Current.Type != TokenType.RightCurlyBracer)
                {
                    throw new ParserException($"Expected {TokenType.RightCurlyBracer} but found {Current.Type}");
                }
            }
            else if (Peek(2).Type == TokenType.Semicolon)
            {
                _position += 2;
                return new StructSyntaxNode(next.Value);
                // forward declaration
            }

        }
        throw new ParserException($"Expected {TokenType.Identifier} but found {next.Type}");
    }

    private SyntaxNode[] ParseMembers()
    {
        throw new InvalidOperationException();
        //List<SyntaxNode> fields = new();
        //while (Current.Type != TokenType.RightCurlyBracer)
        //{
        //    var field = ParseField();
        //    var type = Current;
        //    if (Peek(1).Type == TokenType.Identifier)
        //    {

        //    }
        //}

    }
}

internal class ParserException : Exception
{
    public ParserException(string message) : base(message)
    {
        
    }
}

[DebuggerDisplay("{ToString()}")]
public abstract class SyntaxNode
{
    public virtual void PrettyPrint(int indentation = 0){}
}

public sealed class StructSyntaxNode : SyntaxNode
{
    public string Name { get; }
    public bool Complete { get; }
    public StructSyntaxNode(string name)
    {
        Name = name;
        Complete = false;
    }
}