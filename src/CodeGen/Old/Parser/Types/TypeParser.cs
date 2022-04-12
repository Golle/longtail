using System;
using System.Collections.Generic;
using CodeGen.Parser.Expressions;
using CodeGen.Tokenizer;

namespace CodeGen.Parser.Types;

internal static class TypeParser
{
    public static void Parse(ref TokenCursor cursor, TypeLookupTable types)
    {
        TypeDeclaration? type;
        if (cursor.Is(TokenType.Typedef))
        {
            cursor.Advance();
            type = ParseType(ref cursor, types);
            while (cursor.Not(TokenType.Semicolon))
            {
                if (cursor.Is(TokenType.Comma))
                {
                    cursor.Advance();
                    continue;
                }

                var pointer = false;
                if (cursor.Is(TokenType.Operator, "*"))
                {
                    pointer = true;
                    cursor.Advance();
                }
                
                if (cursor.Not(TokenType.Identifier))
                {
                    throw new ParserException($"Expected identifer at line {cursor.Current.Line} column {cursor.Current.Column}");
                }
                // TODO: not sure how to do this. We want the method to return something. Maybe a typedef declaration with multiple names?
                types.AddTypedef(cursor.Current.Value, type, pointer);
                cursor.Advance();
            }
            cursor.Advance();
        }
        else
        {
            // How should we handle anonomous types?
            type = ParseType(ref cursor, types);
        }
        types.Add(type);
    }

    private static TypeDeclaration ParseType(ref TokenCursor cursor, TypeLookupTable types)
    {
        if (cursor.Is(TokenType.Enum))
        {
            cursor.Advance();
            return ParseEnum(ref cursor, types);
        }

        if (cursor.Is(TokenType.Struct))
        {
            
        }

        if (cursor.Is(TokenType.Identifier))
        {
            var type = types.Find(cursor.Current.Value);
        }

        throw new NotSupportedException();
    }


    private static TypeDeclaration ParseEnum(ref TokenCursor cursor, TypeLookupTable types)
    {
        var name = string.Empty; //TODO: support anonymous names (that are used in typedefs)
        TypeDeclaration? baseType = null;
        if (cursor.Is(TokenType.Identifier))
        {
            name = cursor.Current.Value;
            cursor.Advance();
            if (cursor.Is(TokenType.Colon) && cursor.Peek().Type == TokenType.PrimitiveType)
            {
                cursor.Advance();
                var type = cursor.ReadPrimitiveType();
                baseType = types.Find(type) ?? throw new ParserException($"Failed to find type {cursor.Current.Value}");
                cursor.Advance();
            }
        }
        

        if (cursor.Not(TokenType.LeftCurlyBracer))
        {
            throw new ParserException($"Expected {TokenType.LeftCurlyBracer}");
        }

        cursor.Advance();

        if (cursor.Is(TokenType.RightCurlyBracer))
        {
            throw new ParserException("Empty enum declaration");
        }

        var members = ParseEnumMembers(ref cursor, types);

        if (cursor.Not(TokenType.RightCurlyBracer))
        {
            throw new ParserException($"Expected {TokenType.RightCurlyBracer}");
        }
        cursor.Advance();
        return new EnumTypeDeclaration(name, members, baseType);
    }

    private static EnumMember[] ParseEnumMembers(ref TokenCursor cursor, TypeLookupTable types)
    {
        List<EnumMember> members = new();
        while (cursor.Not(TokenType.RightCurlyBracer))
        {
            var member = ParseEnumMember(ref cursor, types);
            members.Add(member);
            if (cursor.Is(TokenType.Comma))
            {
                cursor.Advance();
            }
        }
        return members.ToArray();
    }

    private static EnumMember ParseEnumMember(ref TokenCursor cursor, TypeLookupTable types)
    {
        if (cursor.Not(TokenType.Identifier))
        {
            throw new ParserException($"Malformed enum member declaration at line {cursor.Current.Line} column {cursor.Current.Column}");
        }

        var name = cursor.Current.Value;
        cursor.Advance();
        if (cursor.Is(TokenType.Equal))
        {
            cursor.Advance();
            return new EnumMember(name, ExpressionParser.Expression(ref cursor, types));
        }
        return new EnumMember(name);
    }
}