using System;
using System.Data;
using System.Diagnostics;

namespace CodeGen;


internal enum TokenKind
{
    // Operators
    Plus,
    Minus,
    Slash,
    Star,

    Identifier,
    Integer,
    Punctation,
    BackSlash,


    // Special
    Invalid,
    EndOfFile,


}

[DebuggerDisplay("{ToString()}")]
internal class Token
{
    public static readonly Token InvalidToken = new(TokenKind.Invalid, string.Empty);
    public TokenKind Kind { get; }
    public string Value { get; }

    public Token(TokenKind kind) : this(kind, string.Empty) { }
    public Token(TokenKind kind, string value)
    {
        Kind = kind;
        Value = value;
    }

    public override string ToString() => $"{Kind}: {Value}";
}


internal class Lexer
{
    private readonly string _input;
    private char Current => _position < _input.Length ? _input[_position] : '\0';

    private int _position;

    private bool HasMore() => _position < _input.Length;
    public Lexer(string input)
    {
        _input = input;
    }

    public Token Lex()
    {
        if (!HasMore())
        {
            return new Token(TokenKind.EndOfFile);
        }
        switch (Current)
        {
            case '*' or '/' or '+' or '-':
                return ParseOperator();
            case >= '0' and <= '9':
                return ParseNumberLiteral();
            case '#':
                return ParsePreProcessor();
            default:
                return ParseIdentifier();
        }

    }

    private Token ParsePreProcessor()
    {
        Advance();
        return Token.InvalidToken;
    }

    private Token ParseIdentifier()
    {
        static bool IsCharacter(char c) => c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z');
        static bool IsNumber(char c) => c is >= '0' and <= '9';
        static bool IsSpecial(char c) => c is '_';
        static bool IsValidIdentifier(char c) => IsSpecial(c) || IsNumber(c) || IsCharacter(c);
        
        var start = _position;
        // Only allow identifiers to start with an underscore or a character
        if (!IsCharacter(Current) || IsSpecial(Current))
        {
            Advance();
            return Token.InvalidToken;
        }
        Advance();

        while (IsValidIdentifier(Current))
        {
            Advance();
        }

        return new Token(TokenKind.Identifier, _input.Substring(start, _position - start));
    }

    private Token ParseNumberLiteral()
    {
        var start = _position;
        while (char.IsNumber(Current))
        {
            Advance();
        }
        return new Token(TokenKind.Integer, _input[start.._position]);
    }

    private Token ParseOperator()
    {
        switch (Current)
        {
            case '*':
                Advance();
                return new Token(TokenKind.Star, "*");
            case '/':
                Advance();
                return new Token(TokenKind.Slash, "/");
            case '+':
                Advance();
                return new Token(TokenKind.Plus, "+");
            case '-':
                Advance();
                return new Token(TokenKind.Minus, "-");
        }

        return Token.InvalidToken;
    }

    private void Advance()
    {
        _position++;
    }
}
