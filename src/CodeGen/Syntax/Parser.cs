using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodeGen.Lexer;
using CodeGen.Logging;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;

namespace CodeGen.Syntax;

internal class Parser
{
    private static readonly Token InvalidToken = new() { Type = TokenType.Invalid };
    private readonly Token[] _tokens;
    private int _position;
    private Token Current => Peek(0);

    private List<TokenType> _functionModifiers = new();
    private Token Peek(int steps)
    {
        var position = _position + steps;
        return position < _tokens.Length ? _tokens[position] : InvalidToken;
    }

    public Parser(string input)
    {
        _tokens = new Tokenizer()
            .Tokenize(input)
            .Where(t => t.Type is not TokenType.NewLine)
            .ToArray();
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
                case TokenType.PreProcessor:
                    SkipPreProcessor();
                    break;

                case TokenType.EndOfFile:
                case TokenType.Invalid:
                    parsing = false;
                    break;
                default:
                    nodes.Add(ParseGlobalStatement());
                    break;
            }
        }
        return new SyntaxTree(nodes.ToArray());
    }

    private void SkipPreProcessor()
    {
        //NOTE(Jens): this is a temporary solution to enable us to parse the C code from the headerfile whileignoring any preprocessor includes, defines or if statements.
        if (Current.Value is "if" or "ifdef" or "ifndef")
        {
            Logger.Trace($"Found {Current} on line: {Current.Line} Column: {Current.Column}");
            _position++;
            while (!(Current.Type == TokenType.PreProcessor && Current.Value == "endif"))
            {
                if (Current.Type == TokenType.EndOfFile)
                {
                    throw new ParserException("Failed to find #endif before end of file");
                }
                if (Current.Type == TokenType.PreProcessor && Current.Value == "if")
                {
                    SkipPreProcessor();
                }
                _position++;
            }
            Logger.Trace($"Found endif on line: {Current.Line} Column: {Current.Column}");
            _position++;
        }

        while (Current.Type != TokenType.PreProcessorEnd)
        {
            _position++;
        }
        // Advance past the end token
        _position++;
    }

    private SyntaxNode ParseGlobalStatement()
    {
        var statement = ParseTypedefStatement();
        while (Current.Type == TokenType.Semicolon)
        {
            _position++;
        }
        return statement;
    }

    private Statement ParseTypedefStatement()
    {
        if (Current.Type != TokenType.Typedef)
        {
            return ParseStatement();
        }


        Statement statement;
        _position++;
        if (IsStructDeclaration())
        {
            //statement = ParseStructDeclaration();
            throw new NotImplementedException("We should support 'typedef struct {} name', but it's not required at the moment.");
        }
        else
        {
            var expression = TryParseTypeExpression() ?? throw new ParserException("Failed to parse the typedef.");

            // Function pointer declaration
            if (Current.Type is TokenType.LeftParenthesis && Peek(1).Type == TokenType.Star)
            {
                _position += 2;
                if (Current.Type != TokenType.Identifier)
                {
                    throw new ParserException($"Expected {TokenType.Identifier} but found {Current.Type} when parsing the typedef function pointer.");
                }

                var name = Current.Value;
                _position += 3; // Skip "Name)(" in the function pointer declaration
                var arguments = ParseArgumentList();
                _position++;
                //NOTE(Jens): this is treating a function pointer as a function declaration.
                //NOTE(Jens): we could add another type for function pointers to make it more clear? What ever makes the C# code gen easier.
                return new FunctionDeclarationStatement(expression, name, arguments, isFunctionPointer: true);

                //NOTE(Jens): do we want the typedef wrapper for function pointers? it's the only way to define a function pointer right so maybe we can discard the typedef?
                //return new TypedefStatement(name, new FunctionDeclarationStatement(expression, name, arguments));
            }

            if (Current.Type == TokenType.Identifier)
            {
                var name = Current.Value;
                _position++;
                statement = new TypedefStatement(name, new ExpressionStatement(expression));
            }
            else
            {
                throw new ParserException($"Expected {TokenType.Identifier} or {TokenType.LeftParenthesis} but found {Current.Type} when parsing typedef");
            }
        }
        return statement;
    }


    private Statement ParseStatement()
    {
        Statement statement;
        if (Current.Type == TokenType.Struct && IsStructDeclaration())
        {
            statement = ParseStructDeclaration();
        }
        else if (Current.Type == TokenType.Enum)
        {
            statement = ParseEnumStatement();
        }
        else
        {
            statement = ParseFunctionModifiers();
        }

        return statement;
    }

    private Statement ParseEnumStatement()
    {
        _position++;
        var name = string.Empty;
        //anonymous enums are possible, this will support both
        if (Current.Type == TokenType.Identifier)
        {
            name = Current.Value;
            _position++;
        }

        if (Current.Type != TokenType.LeftCurlyBracer)
        {
            throw new ParserException($"Expected {TokenType.LeftCurlyBracer} but found {Current.Type} when parsing an enum");
        }
        _position++;
        List<Expression> members = new();

        while (Current.Type != TokenType.EndOfFile)
        {
            if (Current.Type == TokenType.RightCurlyBracer)
            {
                break;
            }
            if (Current.Type == TokenType.Comma)
            {
                _position++;
            }
            //NOTE(Jens): this will support weird syntax, but it's ok since we should not parse anything that has not been verified.
            members.Add(ParseExpression());
        }

        //Trailing commas are possible in Enum declarations
        if (Current.Type == TokenType.Comma)
        {
            _position++;
        }

        if (Current.Type != TokenType.RightCurlyBracer)
        {
            throw new ParserException($"Expected {TokenType.RightCurlyBracer} but found {Current.Type} when parsing an enum");
        }
        _position++;

        return new EnumDeclarationStatement(name, members.ToArray());
    }

    private bool IsStructDeclaration()
    {
        if (Current.Type != TokenType.Struct)
        {
            return false;
        }
        for (var i = 1; i < 10; ++i)
        {
            var token = Peek(i).Type;
            // pointer or a paranthesis indicates that its not a struct
            if (token is TokenType.Star or TokenType.LeftParenthesis)
            {
                return false;
            }

            // found a curly bracer or a semicolon, this is a struct definition
            if (token is TokenType.LeftCurlyBracer or TokenType.Semicolon)
            {
                return true;
            }
        }
        // nothing found after 10 steps, this is not expected.
        throw new ParserException("Could not find a token that can determine if it's a struct definition or struct return type.");
    }

    
    // NOTE(Jens): maybe this can be used for a more general function modifier? 
    private Statement ParseFunctionModifiers()
    {
        while (true)
        {
            if (Current.Type == TokenType.DeclSpec)
            {
                _position++;
                if (Current.Type != TokenType.LeftParenthesis)
                {
                    throw new ParserException($"Expected {TokenType.LeftParenthesis} but found {Current.Type} when parsing a __declspec statement on Line: {Current.Line} Column: {Current.Column}");
                }
                _position++;
                _functionModifiers.Add(Current.Type);
                _position++;
                if (Current.Type != TokenType.RightParenthesis)
                {
                    throw new ParserException($"Expected {TokenType.RightParenthesis} but found {Current.Type} when parsing a __declspec statement on Line: {Current.Line} Column: {Current.Column}");
                }
                _position++;
                //return new DeclspecStatement(declspecType, ParseExpressionStatement());
            }
            else if (Current.Type == TokenType.Extern)
            {
                _functionModifiers.Add(Current.Type);
                _position++;
            }
            else
            {
                break;
            }
        }
        var expression = ParseExpressionStatement();
        // clear the function modifiers when the expression statement returns.
        _functionModifiers.Clear();
        return expression;
    }

    private Statement ParseExpressionStatement()
    {
        Statement statement;
        //NOTE(Jens) This is not a perfect solution since it will allow bad syntax, like const (1+2) unsigned *& A()
        var expression = ParseExpression();
        if (Current.Type == TokenType.Identifier)
        {
            if (Peek(1).Type == TokenType.LeftParenthesis)
            {
                var identifier = Current.Value;
                // Function declaration
                _position += 2;
                var arguments = ParseArgumentList();
                if (Current.Type != TokenType.RightParenthesis)
                {
                    throw new ParserException($"Expected {TokenType.RightParenthesis} operator but found {Current.Type}");
                }
                _position++;
                if (Current.Type == TokenType.Semicolon)
                {
                    Logger.Trace($"Function declaration: {identifier}");
                    statement = new FunctionDeclarationStatement(expression, identifier, arguments, _functionModifiers.ToArray());
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
                // variable declaration
                var variable = ParseExpression();
                statement = new VariableDeclarationStatement(expression, variable);
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

    private VariableDeclarationStatement[] ParseArgumentList()
    {
        //TODO: use array pool?
        List<VariableDeclarationStatement> arguments = new();
        while (Current.Type != TokenType.RightParenthesis)
        {
            //Note(Jens): this supports weird syntax, but its probably fine for our use case
            var expr = ParseExpression();

            Expression identifier;
            //Named parameter
            if (Current.Type == TokenType.Identifier)
            {
                identifier = ParseAccessorExpression();
            }
            else
            {
                identifier = new IdentifierExpression("");
            }
            arguments.Add(new VariableDeclarationStatement(expr, identifier));

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
        var expression = ParseAccessorExpression();
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

    private Expression ParseAccessorExpression()
    {
        var expression = ParseBinaryExpression();
        if (Current.Type == TokenType.LeftSquareBracket)
        {
            _position++;
            Expression? inside = null;
            if (Current.Type != TokenType.RightSquareBracket)
            {
                inside = ParseExpression();
                if (Current.Type != TokenType.RightSquareBracket)
                {
                    throw new ParserException($"Expected {TokenType.RightSquareBracket} but found {Current.Type} on Line: {Current.Line} Column: {Current.Column}");
                }
            }
            _position++;
            return new ArrayExpression(expression, inside);
        }
        //switch (Current.Type)
        //{
        //    case TokenType.LeftSquareBracket:



        //        break;
        //    case TokenType.LeftParenthesis:

        //        break;
        //    case TokenType.Punctuation:
        //        break;
        //    case TokenType.Pointer:
        //        break;
        //}

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

        //Any literal token, just return
        if (current.Type is TokenType.Boolean or TokenType.String or TokenType.Number or TokenType.Character)
        {
            _position++;
            return new LiteralExpression(current.Type, current.Value);
        }

        // Parenthesis
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

        return TryParseTypeExpression() ?? throw new ParserException($"Primary expression for token type {current.Type} has not been implemented. FIX IT! {Current.Line}::{current.Column}");
    }

    private static bool IsBuiltInType(TokenType type) => type is
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

    private Expression? TryParseTypeExpression()
    {
        // if the expression starts with const, wrap it and return.
        if (Current.Type == TokenType.Const)
        {
            _position++;
            var expression = TryParseTypeExpression() ?? throw new ParserException("Failed to parse type after const expression");
            return new ConstExpression(expression);
        }

        if (Current.Type == TokenType.Struct)
        {
            _position++;
            var expression = TryParseTypeExpression() ?? throw new ParserException("Failed to parse type after struct keyword");
            return new StructExpression(expression);
        }

        //NOTE(Jens): for C99 we don't need class support, implement this if we need it
        //if (Current.Type == TokenType.Class)
        //{

        //}


        Expression? expr;
        // Identifier is probably a custom type
        if (Current.Type == TokenType.Identifier)
        {
            expr = new IdentifierExpression(Current.Value);
            _position++;
        }
        // Built in types 
        else if (IsBuiltInType(Current.Type))
        {
            var modifierCount = 0;
            Span<TokenType> typeModifiers = stackalloc TokenType[10];
            while (IsBuiltInType(Current.Type))
            {
                typeModifiers[modifierCount++] = Current.Type;
                _position++;
            }
            expr = new BuiltInTypeExpression(typeModifiers[..modifierCount]);
        }
        else
        {
            return null;
        }
        return ParsePointerOrReferenceExpression(expr);
    }

    private Expression ParsePointerOrReferenceExpression(Expression expr)
    {
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
            else if (Current.Type == TokenType.Const)
            {
                expr = new ConstExpression(expr);
            }
            else
            {
                break;
            }
            _position++;
        }
        return expr;
    }

    private Statement ParseStructDeclaration()
    {
        //TODO: add anonymous types
        _position++;
        if (Current.Type != TokenType.Identifier)
        {
            throw new ParserException($"Expected {TokenType.Identifier} when parsing a struct but found {Current.Type}");
        }
        var name = Current.Value;
        Logger.Trace($"Struct declaration: {name}");
        _position++;

        // Full struct definition
        if (Current.Type == TokenType.LeftCurlyBracer)
        {
            _position++;
            var members = ParseMembers();

            // struct definition
            if (Current.Type != TokenType.RightCurlyBracer)
            {
                throw new ParserException($"Expected {TokenType.RightCurlyBracer} but found {Current.Type}");
            }
            _position++;
            return new StructDeclarationStatement(name, members);
        }

        // forward declaration
        if (Current.Type == TokenType.Semicolon)
        {
            _position++;
            return new StructDeclarationStatement(name);
        }

        throw new ParserException($"Expected {TokenType.LeftCurlyBracer} or {TokenType.Semicolon} but found {Current.Type}");
    }

    private VariableDeclarationStatement[] ParseMembers()
    {
        List<VariableDeclarationStatement> members = new();
        while (Current.Type != TokenType.RightCurlyBracer)
        {
            var type = ParseExpression();
            if (Current.Type != TokenType.Identifier)
            {
                throw new ParserException($"Expected {TokenType.Identifier} but found {Current.Type} when parsing struct members on Line: {Current.Line} Column: {Current.Column}");
            }
            var identifier = ParseAccessorExpression();
            members.Add(new VariableDeclarationStatement(type, identifier));

            if (Current.Type != TokenType.Semicolon)
            {
                throw new ParserException($"Expected {TokenType.Semicolon} but found {Current.Type} when closing the struct member definition. Line: {Current.Line} Column: {Current.Column}");
            }
            _position++;
        }
        return members.ToArray();
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
    public virtual void PrettyPrint(IPrettyPrint print, int indentation = 0)
    {
        print.Write($"{nameof(PrettyPrint)} has not been implemented for {GetType().Name}", indentation);
    }
}

