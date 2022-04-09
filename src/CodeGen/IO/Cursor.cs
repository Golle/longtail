using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CodeGen.IO;

internal sealed class Cursor
{
    public const char InvalidCharacter = char.MaxValue;
    public uint Line { get; private set; } = 1;
    public uint Column { get; private set; } = 1;


    private readonly char[] _buffer;
    private readonly int _size;

    private uint _position;

    public Cursor(string source)
    {
        _buffer = source.ToCharArray();
        _size = _buffer.Length;
    }

    public bool Advance(int count = 1)
    {
        for (var i = 0; i < count; ++i)
        {
            if (!MoveNext())
            {
                return false;
            }
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext() => Read(out var _);

    /// <summary>
    /// Returns the current character and advances the position
    /// </summary>
    /// <returns>true if not EOF</returns>
    public bool Read([NotNullWhen(true)] out char? character)
    {
        Unsafe.SkipInit(out character);
        if (_position >= _size)
        {
            return false;
        }
        character = _buffer[_position];

        if (character == '\n')
        {
            Line++;
            Column = 1;
        }
        else
        {
            Column++;
        }

        //if (character == '\r' )
        //{
        //    // if it's \r, move the pointer one more step
        //    if (_position + 1 < _size && _buffer[_position + 1] == '\n')
        //    {
        //        _position++;
        //    }
        //    Line++;
        //    Column = 0;
        //}
        //else
        //{
        //    Column++;
        //}
        _position++;
        return true;
    }


    public char Peek(int offset = 1) => Peek(offset, out var c) ? c.Value : InvalidCharacter;

    public bool Peek(int offset, [NotNullWhen(true)] out char? character)
    {
        Unsafe.SkipInit(out character);
        var pos = _position + offset;
        if (pos >= _size)
        {
            return false;
        }

        character = _buffer[pos];
        return true;
    }

    public char Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Peek(0, out var c) ? c.Value : InvalidCharacter;
    }

    public ReadOnlySpan<char> GetSubstring(int length)
    {
        //TODO: handle overflow?
        return new ReadOnlySpan<char>(_buffer, (int)_position, length);
    }

    public ReadOnlySpan<char> GetSurroundingString(int length = 10)
    {
        var start = Math.Max(_position - length, 0);

        return new ReadOnlySpan<char>(_buffer, (int)start, length*2);
    }
}