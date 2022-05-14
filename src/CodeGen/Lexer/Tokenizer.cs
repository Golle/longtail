using System;
using System.Collections.Generic;
using System.IO;

namespace CodeGen.Lexer;

internal class Tokenizer
{
    private readonly bool _skipComments;
    private readonly IDictionary<string, TokenType> _tokenLookupTable;

    public Tokenizer(bool skipComments = true, IDictionary<string, TokenType>? tokenLookupTable = null)
    {
        _skipComments = skipComments;
        _tokenLookupTable = tokenLookupTable ?? new Dictionary<string, TokenType>();
    }

    public IReadOnlyList<Token> Tokenize(string input)
    {
        Cursor cursor = new(input);
        List<Token> tokens = new(100_000);
        var isPreProcessorLine = false;
        do
        {
            var token = new Token(cursor.Line, cursor.Column);

            switch (cursor.Current)
            {
                case '\r':
                    // Completely ignore \r
                    continue;
                case '"':
                    StringLiteral(ref cursor, ref token);
                    break;
                case '\'':
                    CharacterLiteral(ref cursor, ref token);
                    break;
                case '/':
                    if (cursor.Peek() is '*' or '/')
                    {
                        Comment(ref cursor, ref token);
                    }
                    else
                    {
                        Operator(ref cursor, ref token);
                    }
                    break;
                case '*' or '!' or '+' or '-' or '^' or '&' or '=' or '?' or '~' or ':' or '<' or '>' or '%' or '|':
                    Operator(ref cursor, ref token);
                    break;
                case ',':
                    token.Type = TokenType.Comma;
                    break;
                case ';':
                    token.Type = TokenType.Semicolon;
                    break;
                case '.':
                    if (cursor.Peek() == '.' && cursor.Peek(2) == '.')
                    {
                        token.Type = TokenType.VariadicArgument;
                        cursor.Advance(2);
                    }
                    else
                    {
                        token.Type = TokenType.Punctuation;
                    }
                    break;
                case '\\':
                    if (isPreProcessorLine)
                    {
                        var count = 1;
                        while (cursor.Peek(count) == ' ')
                        {
                            count++;
                        }
                        
                        // NOTE(Jens): this might skip an additional line
                        if (cursor.Peek(count) is '\n' or '\r')
                        {
                            cursor.Advance(count);
                        }
                        if (cursor.Peek() is '\n' or '\r')
                        {
                            cursor.Advance();
                        }
                        continue;
                    }
                    token.Type = TokenType.Backslash;
                    break;
                case '{' or '}' or '[' or ']' or '(' or ')':
                    token.Type = Bracket(cursor.Current);
                    break;
                case '\n':
                    if (isPreProcessorLine)
                    {
                        // Replace new line with preprocessor end
                        token.Type = TokenType.PreProcessorEnd;
                        isPreProcessorLine = false;
                    }
                    else
                    {
                        token.Type = TokenType.NewLine;
                    }
                    break;
                case '#':
                    PreProcessor(ref cursor, ref token);
                    isPreProcessorLine = true;
                    break;
                case >= '0' and <= '9':
                    NumberLiteral(ref cursor, ref token);
                    break;
                case ' ' or '\t' or '\f':
                    break;
                default:
                    Identifier(ref cursor, ref token);
                    if (_tokenLookupTable.TryGetValue(token.Value, out var newToken))
                    {
                        token.Type = newToken;
                    }
                    break;
            }

            if (_skipComments && token.Type == TokenType.Comment)
            {
                continue;
            }
            if (token.Type != TokenType.Unknown) // Ignore space, tabs etc that does't affect the code
            {
                tokens.Add(token);
            }
        } while (cursor.Advance());

        tokens.Add(new Token(cursor.Line, cursor.Column)
        {
            Type = TokenType.EndOfFile
        });
        return tokens;
    }


    private static void NumberLiteral(ref Cursor cursor, ref Token token)
    {
        static bool IsNumber(char c) => c is >= '0' and <= '9';
        static bool IsValidNumberLiteral(char c) => c is 'U' or 'L' or 'F' or 'u' or 'l' or 'f' or '.' || IsNumber(c);
        static bool IsValidHexLiteral(char c) => c is >= 'A' and <= 'F' || c is >= 'a' and <= 'f' || IsValidNumberLiteral(c);

        Span<char> buffer = stackalloc char[128];
        var i = 0;
        buffer[i++] = cursor.Current;
        if (cursor.Peek() is 'x' or 'X')
        {
            cursor.Advance();
            buffer[i++] = cursor.Current;
            while (IsValidHexLiteral(cursor.Peek()))
            {
                cursor.Advance();
                buffer[i++] = cursor.Current;
            }
        }
        else
        {
            while (IsValidNumberLiteral(cursor.Peek()))
            {
                cursor.Advance();
                buffer[i++] = cursor.Current;
            }
        }

        token.Type = TokenType.Number;
        token.Value = new string(buffer[..i]);
    }

    private static void CharacterLiteral(ref Cursor cursor, ref Token token)
    {
        if (cursor.Peek(2) != '\'')
        {
            throw new FormatException("A character literal can only be a single letter.");
        }

        token.Value = cursor.Peek().ToString();
        token.Type = TokenType.Character;
        cursor.Advance(2);
    }

    private static void StringLiteral(ref Cursor cursor, ref Token token)
    {
        const int maxBufferSize = 2048;
        Span<char> buffer = stackalloc char[maxBufferSize];

        // TODO: add support for escaped strings like "this is a \"string\" yepp"
        var i = 0;
        while (cursor.Peek() != '\"')
        {
            cursor.Advance();
            buffer[i++] = cursor.Current;
            if (i >= maxBufferSize)
            {
                throw new InternalBufferOverflowException("The character buffer overflowed, increase the buffer.");
            }
        }

        token.Type = TokenType.String;
        token.Value = new string(buffer[..i]);
        cursor.Advance();
    }

    private static void PreProcessor(ref Cursor cursor, ref Token token)
    {
        static bool IsCharacter(char c) => (c is >= 'a' and <= 'z') || (c is >= 'A' and <= 'Z');
        token.Type = TokenType.PreProcessor;

        // TODO: add support for defines that have space in them like " #  ifndef"
        Span<char> identifer = stackalloc char[128];
        var i = 0;
        while (IsCharacter(cursor.Peek()))
        {
            cursor.Advance();
            identifer[i++] = cursor.Current;
        }

        token.Value = new string(identifer[..i]);
    }

    private static void Identifier(ref Cursor cursor, ref Token token)
    {
        static bool IsCharacter(char c) => (c is >= 'a' and <= 'z') || (c is >= 'A' and <= 'Z');
        static bool IsNumber(char c) => c is >= '0' and <= '9';
        static bool IsSpecial(char c) => c is '_';
        static bool IsValidIdentifier(char c) => IsCharacter(c) || IsNumber(c) || IsSpecial(c);
        if (!(IsCharacter(cursor.Current) || IsSpecial(cursor.Current)))
        {
            throw new InvalidOperationException("Identifier must start with a character");
        }

        const int maxIdentifierSize = 256;
        Span<char> identifier = stackalloc char[maxIdentifierSize]; // TODO: verify identifers, if its longer than 256 characters increase this
        var i = 0;
        identifier[i++] = cursor.Current;
        while (IsValidIdentifier(cursor.Peek()))
        {
            cursor.Advance();
            identifier[i++] = cursor.Current;
            if (i >= maxIdentifierSize)
            {
                throw new NotSupportedException("Max Identifer size reached.");
            }
        }


        var span = identifier[..i];
        if (BuiltInTypesTable.TryGetType(span, out var type))
        {
            token.Type = type.Type;
            token.Value = type.Value;
        }
        else if (StatementsTable.TryGetStatement(span, out var statement))
        {
            token.Type = statement.Type;
            token.Value = statement.Value;
        }
        else
        {
            (token.Type, token.Value) = CppKeywords.Translate(span);
        }
    }

    private static void Comment(ref Cursor cursor, ref Token token)
    {
        token.Type = TokenType.Comment;
        if (cursor.Peek() == '/') // inline comment
        {
            while (cursor.Current is not '\n')
            {
                if (!cursor.Advance())
                {
                    // End of file reached.
                    return;
                }
            }
        }
        else // block comment
        {
            static bool EndOfComment(ref Cursor cursor) => cursor.Current == '*' && cursor.Peek() == '/';

            while (!EndOfComment(ref cursor))
            {
                cursor.Advance();
            }
            cursor.Advance();
        }

    }

    private static void Operator(ref Cursor cursor, ref Token token)
    {
        //Longest operator is 3 characters
        Span<char> characterStack = stackalloc char[3];
        for (var i = 0; i < 3; ++i)
        {
            characterStack[i] = cursor.Peek(i);
        }
        if (OperatorTable.TryGetOperator(characterStack, out var op))
        {
            token.Value = op.Value;
            token.Type = op.Type;
            cursor.Advance(token.Value.Length - 1);
        }
    }

    private static TokenType Bracket(char c) => c switch
    {
        '(' => TokenType.LeftParenthesis,
        ')' => TokenType.RightParenthesis,
        '{' => TokenType.LeftCurlyBracer,
        '}' => TokenType.RightCurlyBracer,
        '[' => TokenType.LeftSquareBracket,
        ']' => TokenType.RightSquareBracket,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
}