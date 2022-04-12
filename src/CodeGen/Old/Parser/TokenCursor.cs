using System;
using CodeGen.Tokenizer;

namespace CodeGen.Parser;

internal ref struct TokenCursor
{
    private readonly ReadOnlySpan<Token> _tokens;
    private readonly int _length;
    private int _position;
    public bool HasMore => _position < _length;
    public ref readonly Token Current => ref _tokens[_position];

    public TokenCursor(ReadOnlySpan<Token> tokens)
    {
        _tokens = tokens;
        _length = tokens.Length;
        _position = 0;
    }

    public ref readonly Token Peek(int offset = 1, bool skipNewLine = true)
    {
        var position = _position;
        // Go back or forward
        var increment = offset > 0 ? 1 : -1;

        // Make sure the offset is positive
        offset *= increment;

        for (var i = 1; i <= offset; ++i)
        {
            if (!PeekNext(ref position, increment, skipNewLine))
            {
                throw new IndexOutOfRangeException("Can't peek past the last token");
            }
        }
        return ref _tokens[position];
    }

    private bool PeekNext(ref int position, int increment, bool skipNewLine)
    {
start:
        position += increment;
        if (position >= _length || position < 0)
        {
            return false;

        }

        if (skipNewLine && _tokens[position].Type == TokenType.NewLine)
        {
            goto start;
        }
        return true;
    }

    public bool Advance(uint count, bool skipNewLine = true)
    {
        for (var i = 0; i < count; ++i)
        {
            if (!Advance(skipNewLine))
            {
                return false;
            }
        }

        return true;
    }

    public bool Advance(bool skipNewLine = true)
    {
start:
        if (_position + 1 >= _length)
        {
            return false;
        }

        _position++;
        if (skipNewLine && _tokens[_position].Type == TokenType.NewLine)
        {
            goto start;
        }

        return true;
    }

    /// <summary>
    /// Find the next token by type and optional value. This will always count new lines, so if you're using this to advance the cursor make sure you set skip new lines to false.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns>The amount to move forward to reach this token</returns>
    public int FindNext(TokenType type, string? value = null)
    {
        // Increment the current position with 1 since we're looking for the next token.
        for (var position = _position + 1; position < _length; ++position)
        {
            // Check the type and if there's a value specified check that as well.
            if (_tokens[position].Type == type && (value == null || _tokens[position].Value == value))
            {
                return position - _position;
            }
        }
        return -1;
    }
}