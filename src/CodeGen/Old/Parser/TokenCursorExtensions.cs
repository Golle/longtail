using System.Runtime.CompilerServices;
using System.Text;
using CodeGen.Tokenizer;

namespace CodeGen.Parser;

internal static class TokenCursorExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Is(this in TokenCursor cursor, TokenType type) => cursor.Current.Type == type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Is(this in TokenCursor cursor, TokenType type, string value) => cursor.Current.Type == type && cursor.Current.Value == value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Not(this in TokenCursor cursor, TokenType type) => cursor.Current.Type != type;


    public static string ReadPrimitiveType(this ref TokenCursor cursor)
    {
        // TODO: can improve the perf/allocations by using an out parameter and stackalloc from the caller
        var builder = new StringBuilder();
        while (true)
        {
            builder.Append(cursor.Current.Value);
            if (cursor.Peek().Type == TokenType.PrimitiveType)
            {
                cursor.Advance();
                builder.Append(' ');
            }
            else
            {
                break;
            }
        }
        return builder.ToString();
    }
}