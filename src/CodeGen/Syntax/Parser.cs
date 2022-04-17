using System;
using System.Collections.Generic;
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
    private Token Peek(int steps)
    {
        var position = _position + steps;
        return position < _tokens.Length ? _tokens[position] : InvalidToken;
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
        var assigment = ParseAssignmentExpression();

        return assigment;

    }

    private Expression ParseAssignmentExpression()
    {
        var expression = ParseBinaryExpression();
        switch (Current.Type)
        {
            case TokenType.AmpEqual:
            case TokenType.PipeEqual:
            case TokenType.Equal:
            case TokenType.GreaterEqual:
            case TokenType.LessEqual:
            case TokenType.BangEqual:
            case TokenType.MinusEqual:
            case TokenType.PlusEqual:
            case TokenType.StarEqual:
            case TokenType.SlashEqual:
            case TokenType.PercentEqual:
            case TokenType.CaretEqual:
                var token = Current;
                _position++;
                var right = ParseExpression();
                expression = new AssigmentExpression(expression, right, token.Value);
                break;
        }

        return expression;

    }

    private Expression ParseBinaryExpression(int previousPrecedence = 0)
    {
        Expression primary;
        var unaryOperatorPrecedence = Current.UnaryOperatorPrecedence();
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= previousPrecedence)
        {
            var token = Current;
            _position++;
            var operand = ParseBinaryExpression(unaryOperatorPrecedence);
            primary = new UnaryExpression(token.Value, operand);
        }
        else
        {
            primary = ParsePrimaryExpression();
        }

        while (true)
        {
            var operatorPrecedence = Current.BinaryOperatorPrecedence();
            if (operatorPrecedence == 0 || operatorPrecedence <= previousPrecedence)
            {
                break;
            }

            var token = Current;
            _position++;
            var right = ParseBinaryExpression(operatorPrecedence);
            primary = new BinaryExpression(primary, right, token.Value);
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
    public virtual void PrettyPrint(int indentation = 0) { }
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