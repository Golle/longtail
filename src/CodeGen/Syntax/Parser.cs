using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;

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
                    nodes.Add(ParseGlobalStatement());
                    break;
            }
            _position++;
        }
        return new SyntaxTree(nodes.ToArray());
    }


    private SyntaxNode ParseGlobalStatement()
    {
        var statement = ParseStatement();

        return statement;
    }

    private Statement ParseStatement()
    {
        Statement statement;
        var expression = ParseExpression(); //NOTE(Jens) This is not a perfect solution since it will allow bad syntax, like const (1+2) unsigned *& A()
        if (Current.Type == TokenType.Identifier)
        {
            var identifier = Current.Value;
            var next = Peek(1);
            if (next.Type == TokenType.Semicolon)
            {
                _position++;
                statement = new VariableDeclarationStatement(expression, identifier, null);
            }
            else if (next.Type == TokenType.LeftParenthesis)
            {
                _position += 2;
                var arguments = ParseArgumentList();
                if (Current.Type != TokenType.RightParenthesis)
                {
                    throw new ParserException($"Expected {TokenType.RightParenthesis} operator but found {Current.Type}");
                }
                _position++;
                if (Current.Type == TokenType.Semicolon)
                {
                    statement =new FunctionDeclarationStatement(expression, identifier, arguments);
                }
                else if (Current.Type == TokenType.LeftCurlyBracer)
                {
                    throw new NotImplementedException("Function body parsing has not been implemented yet");
                }
                else
                {
                    throw new ParserException($"Invalid token found {Current.Type}");
                }
            }
            else
            {
                _position+=2;
                statement = new VariableDeclarationStatement(expression, identifier, ParseExpression());
            }
        }
        else
        {
            statement = new ExpressionStatement(expression);
        }
        
        // Not sure where to handle this
        if (Current.Type != TokenType.Semicolon)
        {
            throw new ParserException($"Expected {TokenType.Semicolon} at the end of the expression but found a {Current.Type}");
        }
        _position++;

        return statement;
    }
    
    private FunctionDeclarationArgument[] ParseArgumentList()
    {
        // NOTE(jens): keep it simple now and only support the most basic types (single identifer + name)
        //TODO: use array pool
        List<FunctionDeclarationArgument> arguments = new();
        while (Current.Type != TokenType.RightParenthesis)
        {
            var type = Peek(0);
            var identifier = Peek(1);
            var name = string.Empty;
            // support empty type declarations, ex func(void, int);
            if (identifier.Type == TokenType.Identifier)
            {
                name = identifier.Value;
                _position += 2;
            }
            else
            {
                _position++;
            }

            arguments.Add(new FunctionDeclarationArgument(type.Value, name));
            if (Current.Type != TokenType.Comma)
            {
                break;
            }
            _position++;
        }

        return arguments.ToArray();
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
            case TokenType.GreaterGreaterEqual:
            case TokenType.LessLessEqual:
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

        if (current.Type == TokenType.Const)
        {
            _position++;
            return new ConstExpression(ParsePrimaryExpression());
        }
        if (IsBuiltInType(current.Type))
        {
            var modifierCount = 0;
            Span<TokenType> typeModifiers = stackalloc TokenType[10];

            while (IsBuiltInType(Current.Type))
            {
                typeModifiers[modifierCount++] = Current.Type;
                _position++;
            }
            Expression expr = new BuiltInTypeExpression(typeModifiers[..modifierCount]);
            while (true)
            {
                if (Current.Type == TokenType.Star)
                {
                    expr = new PointerTypeExpression(expr);
                }
                else if (Current.Type == TokenType.Amp)
                {
                    expr = new ReferenceTypeExpression(expr);
                }
                else
                {
                    break;
                }
                _position++;
            }
            return expr;
        }

        throw new ParserException($"Primary expression for token type {current.Type} has not been implemented. FIX IT!");

        static bool IsBuiltInType(TokenType type) => type is
            TokenType.Signed or
            TokenType.Unsigned or
            TokenType.Int or
            TokenType.Long or
            TokenType.Char or
            TokenType.Bool or
            TokenType.Float or
            TokenType.Double or
            TokenType.Void
            ;
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