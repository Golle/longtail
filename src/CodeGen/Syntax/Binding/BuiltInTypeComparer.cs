using System;
using System.Collections.Generic;
using CodeGen.Lexer;

namespace CodeGen.Syntax.Binding;

internal class BuiltInTypeComparer : IEqualityComparer<TokenType[]>
{
    public bool Equals(TokenType[]? x, TokenType[]? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x.Length != y.Length)
        {
            return false;
        }
        if (x.Length == 1)
        {
            return x[0] == y[0];
        }


        Span<TokenType> buffer = stackalloc TokenType[x.Length];
        x.CopyTo(buffer);

        foreach (var tokenType in y)
        {
            var found = false;
            for (var i = 0; i < buffer.Length; ++i)
            {
                if (buffer[i] == tokenType)
                {
                    buffer[i] = 0;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                return false;
            }
        }
        return true;

        var result = 0;
        for (var i = 0; i < x.Length; ++i)
        {
            unchecked
            {
                result = x[i] - y[i];
            }
        }

        
        return result == 0;
    }

    public int GetHashCode(TokenType[] obj)
    {
        var result = 0;
        foreach (var tokenType in obj)
        {
            result += (int)tokenType;
        }
        return result;
    }
}