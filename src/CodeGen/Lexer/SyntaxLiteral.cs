using System;
using System.Linq;
using CodeGen.IO;
using CodeGen.Logging;

namespace CodeGen.Lexer;

internal class SyntaxLiteral
{
    private static readonly char[] ValidIntegerSymbols = { '.', 'L', 'U', 'l', 'u', 'f', 'F' };
    public static bool ReadStringLiteral(Cursor cursor, ref SyntaxToken token)
    {
        var current = cursor.Current;
        if (current != '"' && current != '\'')
        {
            Logger.Error("Trying to read a string that isn't surrounded by quotes.");
            return false;
        }

        var i = 0;
        while (true)
        {
            var character = cursor.Peek(i + 1);
            if (character == Cursor.InvalidCharacter)
            {
                throw new InvalidOperationException("End of file reached before finding the end of the string.");
            }

            if (character is '"' or '\'')
            {
                break;
            }
            i++;
        }

        if (i == 0)
        {
            token.Value = string.Empty;
            cursor.Advance(2);
        }
        else
        {
            cursor.Advance();
            token.Value = new string(cursor.GetSubstring(i));
            cursor.Advance(i);
        }

        token.Type = SyntaxTokenType.String;
        return true;
    }

    public static bool ReadIntegerLiteral(Cursor cursor, ref SyntaxToken token)
    {
        var current = cursor.Current;
        if (current is < '0' or > '9')
        {
            Logger.Error("Trying to read a integer that doesn't start with a number");
            return false;
        }

        if (current == '0' && cursor.Peek(1) == 'x')
        {
            // hex
            cursor.Advance(2);
            var length = GetIntegerLength(cursor);
            token.Value = $"0x{cursor.GetSubstring(length).ToString()}";
            cursor.Advance(length);
        }
        else
        {
            var length = GetIntegerLength(cursor);
            token.Value = new(cursor.GetSubstring(length));
            cursor.Advance(length);
        }

        token.Type = SyntaxTokenType.Integer;

        static int GetIntegerLength(Cursor cursor)
        {
            var i = 1;
            while (true)
            {
                var peek = cursor.Peek(i);
                if (peek is < '0' or > '9')
                {
                    if (!ValidIntegerSymbols.Contains(peek))
                    {
                        return i;
                    }
                }
                i++;
            }
        }
        return true;
    }

    public static bool ReadComment(Cursor cursor, ref SyntaxToken token)
    {
        var nextChar = cursor.Peek();

        var length = nextChar switch
        {
            '*' => GetBlockCommentLength(cursor),
            '/' => GetInlineCommentLength(cursor),
            _ => 0
        };

        if (length != 0)
        {
            token.Type = SyntaxTokenType.Comment;
            token.Value = new string(cursor.GetSubstring(length));
            cursor.Advance(length - 1);
            return true;
        }

        return false;

        static int GetBlockCommentLength(Cursor cursor)
        {
            var i = 0;
            while (true)
            {
                var next = cursor.Peek(i);
                if (next == '*' && cursor.Peek(i + 1) == '/')
                {
                    return i + 2;
                }

                if (next == Cursor.InvalidCharacter)
                {
                    Logger.Error("EOF reached without finding the end of the block comment");
                    return 0;
                }
                i++;
            }
        }

        static int GetInlineCommentLength(Cursor cursor)
        {
            var i = 0;
            while (true)
            {
                // Find the end of the line (or the end of the file if the comment is on the last line)
                if (cursor.Peek(i) is '\n' or Cursor.InvalidCharacter)
                {
                    return i;
                }
                i++;
            }
        }
    }

    public static bool ReadIdentifier(Cursor cursor, ref SyntaxToken token)
    {
        static bool IsLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
        static bool IsNumber(char c) => c is >= '0' and <= '9';
        static bool IsUnderscore(char c) => c is '_';
        static bool IsPreprocessorIdentifier(char c) => c is '#';

        var current = cursor.Current;
        if (!IsLetter(current) && !IsUnderscore(current) && !IsPreprocessorIdentifier(current))
        {
            Logger.Error("Identifier must start with _ or a letter");
            return false;
        }

        var i = 1;
        while (true)
        {
            var next = cursor.Peek(i);
            if (!IsLetter(next) && !IsUnderscore(next) && !IsNumber(next) && !IsPreprocessorIdentifier(next))
            {
                break;
            }
            i++;
        }

        token.Type = SyntaxTokenType.Identifier;
        token.Value = new string(cursor.GetSubstring(i));
        cursor.Advance(i - 1);

        return true;
    }

    public static bool ReadPreprocessorDirective(Cursor cursor, ref SyntaxToken token)
    {
        static bool IsLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';

        var current = cursor.Current;

        if (current != '#')
        {
            Logger.Error("PreProcessor directive must start with a #");
            return false;
        }
        cursor.Advance();

        while (cursor.Current == ' ')
        {
            cursor.Advance();
        }

        var i = 0;
        while (true)
        {
            if (!IsLetter(cursor.Peek(i)))
            {
                break;
            }
            i++;
        }

        var value = cursor.GetSubstring(i);
        cursor.Advance(i);

        if (!Enum.TryParse(value, true, out PreProcessorDirective directive))
        {
            Logger.Error($"Unsupported preprocessor directive: {value.ToString()}");
            token.Type = SyntaxTokenType.PreprocessorDirective;
            token.PreProcessorDirective = PreProcessorDirective.Unknown;
            token.Value = new string(value);
            return false;
        }

        token.Type = SyntaxTokenType.PreprocessorDirective;
        token.PreProcessorDirective = directive;

        return true;
    }

    public static bool ReadOperator(Cursor cursor, ref SyntaxToken token)
    {
        static bool IsOperator(char c) => c is '+' or '-' or '*' or '/' or '%' or '=' or '|' or '<' or '>' or '&' or '~';
        static bool IsSimpleOperator(char c) => c is '(' or ')' or '{' or '}' or '[' or ']' or ';' or '?' or '.' or ',' or '\\' or ':';

        var current = cursor.Current;
        if (IsSimpleOperator(current))
        {
            token.Type = SyntaxTokenType.SimpleOperator;
            token.SingleValue = current;
            return true;
        }

        if (current is '!')
        {
            var next = cursor.Peek();
            if (next == '=')
            {
                token.Type = SyntaxTokenType.Operator;
                token.Value = "!=";
                cursor.Advance();
            }
            else
            {
                token.Type = SyntaxTokenType.SimpleOperator;
                token.SingleValue = '!';
            }
            return true;
        }

        if (IsOperator(current))
        {
            var operatorCount = 0;
            Span<char> operators = stackalloc char[10];
            while (operatorCount < 10)
            {
                var next = cursor.Peek(operatorCount);
                if (IsOperator(next))
                {
                    operators[operatorCount++] = next;
                }
                else
                {
                    break;
                }
            }

            if (operatorCount == 1)
            {
                token.Type = SyntaxTokenType.SimpleOperator;
                token.SingleValue = current;
                return true;
            }
            token.Type = SyntaxTokenType.Operator;
            token.Value = new string(operators[..operatorCount]);
            cursor.Advance(operatorCount);
            return true;
        }

        return false;
    }
}