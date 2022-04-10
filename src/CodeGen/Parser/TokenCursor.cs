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
        for (var i = 1; i <= offset; ++i)
        {
            if (!PeekNext(ref position, skipNewLine))
            {
                throw new IndexOutOfRangeException("Can't peek past the last token");
            }
        }
        return ref _tokens[position];
    }

    private bool PeekNext(ref int position, bool skipNewLine)
    {
        start:
        position++;
        if (position >= _length)
        {
            return false;
            
        }

        if (skipNewLine && _tokens[position].Type == TokenType.NewLine)
        {
            goto start;;
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
}