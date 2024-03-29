﻿using System;
using System.Runtime.CompilerServices;

namespace CodeGen.Lexer;

internal static class CppKeywords
{
    private static readonly string[] ClassModifiers = { "public", "protected", "private" };
    private static readonly string[] Modifiers = { "explicit", "volatile", "friend", "final", "virtual", "noexcept", "inline" };
    private static readonly string[] Others = { "constexpr", "__unaligned", "__extension__" };
    private static readonly string[] Booleans = { "true", "false", "TRUE", "FALSE" };
    private static readonly string[] Null = { "nullptr", "NULL" };

    private const string Class = "class";
    private const string Struct = "struct";
    private const string Enum = "enum";
    private const string Union = "union";
    private const string Typedef = "typedef";
    private const string This = "this";
    private const string DeclSpec = "__declspec";
    private const string ForceInline = "__forceinline";
    private const string DllImport = "dllimport";
    private const string DllExport = "dllexport";
    private const string Auto = "auto";
    private const string StaticAssert = "static_assert";
    private const string CPlusPlus = "__cplusplus";

    private const string Static = "static";
    private const string Const = "const";
    private const string Extern = "extern";

    private static readonly string[] CallTypes = { "__cdecl", "__stdcall", "__fastcall" };

    public static (TokenType, string) Translate(ReadOnlySpan<char> identifier)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsMatch(string value, ReadOnlySpan<char> identifier) => value.AsSpan().Equals(identifier, StringComparison.Ordinal);
        static bool MatchesAny(string[] values, ReadOnlySpan<char> identifer, out string result)
        {
            Unsafe.SkipInit(out result);
            foreach (var value in values)
            {
                if (IsMatch(value, identifer))
                {
                    result = value;
                    return true;
                }
            }
            return false;
        }
        if (IsMatch(Class, identifier))
        {
            return (TokenType.Class, string.Empty);
        }
        if (IsMatch(Struct, identifier))
        {
            return (TokenType.Struct, string.Empty);
        }
        if (IsMatch(Enum, identifier))
        {
            return (TokenType.Enum, string.Empty);
        }
        if (IsMatch(Union, identifier))
        {
            return (TokenType.Union, string.Empty);
        }
        if (IsMatch(Typedef, identifier))
        {
            return (TokenType.Typedef, string.Empty);
        }
        if (IsMatch(This, identifier))
        {
            return (TokenType.This, string.Empty);
        }

        if (IsMatch(DllImport, identifier))
        {
            return (TokenType.DllImport, string.Empty);
        }

        if (IsMatch(DllExport, identifier))
        {
            return (TokenType.DllExport, string.Empty);
        }

        if (MatchesAny(ClassModifiers, identifier, out var modifier))
        {
            return (TokenType.ClassModifier, modifier);
        }

        if (MatchesAny(CallTypes, identifier, out var callType))
        {
            return (TokenType.CallType, callType);
        }

        if (MatchesAny(Modifiers, identifier, out var functionModifier))
        {
            return (TokenType.FunctionModifier, functionModifier);
        }

        if (MatchesAny(Others, identifier, out var other))
        {
            return (TokenType.CPPKeyword, other);
        }
        if (MatchesAny(Booleans, identifier, out var boolean))
        {
            return (TokenType.Boolean, boolean.ToLowerInvariant());
        }

        if (MatchesAny(Null, identifier, out var _))
        {
            return (TokenType.Null, string.Empty);
        }

        // Merge DeclSpec and ForceInline ?
        if (IsMatch(DeclSpec, identifier))
        {
            return (TokenType.DeclSpec, string.Empty);
        }

        if (IsMatch(ForceInline, identifier))
        {
            return (TokenType.ForceInline, string.Empty);
        }

        if (IsMatch(StaticAssert, identifier))
        {
            return (TokenType.StaticAssert, string.Empty);
        }

        if (IsMatch(CPlusPlus, identifier))
        {
            return (TokenType.CPlusPlus, string.Empty);
        }

        if (IsMatch(Const, identifier))
        {
            return (TokenType.Const, string.Empty);
        }

        if (IsMatch(Extern, identifier))
        {
            return (TokenType.Extern, string.Empty);
        }
        
        if (IsMatch(Static, identifier))
        {
            return (TokenType.Static, string.Empty);
        }

        //if (IsMatch(Auto, identifier))
        //{
        //    // TODO: if we want something special to happen implement this.
        //    return (TokenType.Identifier, "auto");
        //}
        return (TokenType.Identifier, new string(identifier));
    }
}
