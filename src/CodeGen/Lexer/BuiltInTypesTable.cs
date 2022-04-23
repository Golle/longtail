using System;
using System.Runtime.CompilerServices;

namespace CodeGen.Lexer;

internal record struct TypeDecl(TokenType Type, string Value);
internal static class BuiltInTypesTable
{
    private static readonly TypeDecl[] _types;
    static BuiltInTypesTable()
    {
        _types = new TypeDecl[]
        {
            new(TokenType.Signed, "signed"),
            new(TokenType.Unsigned, "unsigned"),
            new(TokenType.Int, "int"),
            new(TokenType.Long, "long"),
            new(TokenType.Short, "short"),
            new(TokenType.Char, "char"),
            new(TokenType.Bool, "bool"),
            new(TokenType.Double, "double"),
            new(TokenType.Float, "float"),
            new(TokenType.Void, "void"),
        };
    }

    public static bool TryGetType(ReadOnlySpan<char> value, out TypeDecl type)
    {
        Unsafe.SkipInit(out type);
        for (var i = 0; i < _types.Length; ++i)
        {
            if (_types[i].Value.AsSpan().Equals(value, StringComparison.Ordinal))
            {
                type = _types[i];
                return true;
            }
        }
        return false;
    }
}