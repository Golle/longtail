using System;
using System.Collections.Generic;
using CodeGen.Lexer;
using CodeGen.Syntax.Symbols;
using static CodeGen.Lexer.TokenType;

namespace CodeGen.Syntax.Binding;

internal class TypeLookupTable
{
    private List<TypeSymbol> _symbols = new();

    private readonly Dictionary<TokenType[], PrimitiveTypeSymbol> _builtInSymbols = new(new BuiltInTypeComparer());
    
    public TypeLookupTable()
    {
        _builtInSymbols.Add(new[] { TokenType.Char }, new("char", 8));
        _builtInSymbols.Add(new[] { Float }, new("float", 32));
        _builtInSymbols.Add(new[] { TokenType.Double }, new("double", 64));
        _builtInSymbols.Add(new[] { Unsigned, TokenType.Char }, new("unsigned char", 8, true));
        _builtInSymbols.Add(new[] { Bool }, new("bool", 1));
        _builtInSymbols.Add(new[] { Short, Int }, new("short int", 16));
        _builtInSymbols.Add(new[] { Unsigned, Short, Int }, new("unsigned short int", 16, true));
        _builtInSymbols.Add(new[] { Int }, new("int", 32));
        _builtInSymbols.Add(new[] { Unsigned, Int }, new("unsigned int", 32, true));
        _builtInSymbols.Add(new[] { Long, Int }, new("long int", 32));                            //NOTE(Jens): this type is 8 bytes(64bits) on Linux and Mac systems
        _builtInSymbols.Add(new[] { Unsigned, Long, Int }, new("unsigned long int", 32, true));   //NOTE(Jens): this type is 8 bytes(64bits) on Linux and Mac systems
        _builtInSymbols.Add(new[] { Long, Long, Int }, new("long long int", 64));
        _builtInSymbols.Add(new[] { Unsigned, Long, Long, Int }, new("unsigned long long int", 64, true));


        AddAlias(new[] { Short }, new[] { Short, Int });
        AddAlias(new[] { Signed, Short }, new[] { Short, Int });
        AddAlias(new[] { Signed, Short, Int }, new[] { Short, Int });
        AddAlias(new[] { Unsigned, Short }, new[] { Unsigned, Short, Int });
        AddAlias(new[] { Signed }, new[] { Int });
        AddAlias(new[] { Signed, Int }, new[] { Int });
        AddAlias(new[] { Unsigned }, new[] { Unsigned, Int });
        AddAlias(new[] { Long }, new[] { Long, Int });
        AddAlias(new[] { Signed, Long }, new[] { Long, Int });
        AddAlias(new[] { Signed, Long, Int }, new[] { Long, Int });
        AddAlias(new[] { Unsigned, Long }, new[] { Unsigned, Long, Int });
        AddAlias(new[] { Long, Long }, new[] { Long, Long, Int });
        AddAlias(new[] { Signed, Long, Long }, new[] { Long, Long, Int });
        AddAlias(new[] { Signed, Long, Long, Int }, new[] { Long, Long, Int });
        AddAlias(new[] { Unsigned, Long, Long }, new[] { Unsigned, Long, Long, Int });

        void AddAlias(TokenType[] type, TokenType[] baseType)
        {
            _builtInSymbols.Add(type, Find(baseType) ?? throw new Exception($"The base type {string.Join(' ', baseType)} was not found."));
        }
    }
    public TypeLookupTable AddSymbolDefintion(TypeSymbol symbol)
    {


        return this;
    }

    public PrimitiveTypeSymbol? Find(TokenType[] types)
    {

        return _builtInSymbols.TryGetValue(types, out var type) ? type : null;

    }
}