using System;
using System.Collections.Generic;
using CodeGen.Lexer;
using CodeGen.Syntax.Symbols;
using static CodeGen.Lexer.TokenType;

namespace CodeGen.Syntax.Binding;

internal class SymbolLookupTable
{
    private List<Symbol> _symbols = new();

    private readonly Dictionary<TokenType[], PrimitiveTypeSymbol> _primitiveTypes = new(new BuiltInTypeComparer());
    private readonly Dictionary<TokenType, TypeSymbol> _types = new();
    private readonly Dictionary<string, Symbol> _userDefinedTypes = new();

    public SymbolLookupTable()
    {
        _types.Add(Bool, new("bool"));
        _types.Add(TokenType.String, new("string"));
        _types.Add(Number, new("number"));
        _types.Add(Character, new("character"));


        _primitiveTypes.Add(new[] { TokenType.Void }, new("void", 0));
        _primitiveTypes.Add(new[] { TokenType.Char }, new("char", 8));
        _primitiveTypes.Add(new[] { Float }, new("float", 32));
        _primitiveTypes.Add(new[] { TokenType.Double }, new("double", 64));
        _primitiveTypes.Add(new[] { Unsigned, TokenType.Char }, new("unsigned char", 8, true));
        _primitiveTypes.Add(new[] { Bool }, new("bool", 1));
        _primitiveTypes.Add(new[] { Short, Int }, new("short int", 16));
        _primitiveTypes.Add(new[] { Unsigned, Short, Int }, new("unsigned short int", 16, true));
        _primitiveTypes.Add(new[] { Int }, new("int", 32));
        _primitiveTypes.Add(new[] { Unsigned, Int }, new("unsigned int", 32, true));
        _primitiveTypes.Add(new[] { Long, Int }, new("long int", 32));                            //NOTE(Jens): this type is 8 bytes(64bits) on Linux and Mac systems
        _primitiveTypes.Add(new[] { Unsigned, Long, Int }, new("unsigned long int", 32, true));   //NOTE(Jens): this type is 8 bytes(64bits) on Linux and Mac systems
        _primitiveTypes.Add(new[] { Long, Long, Int }, new("long long int", 64));
        _primitiveTypes.Add(new[] { Unsigned, Long, Long, Int }, new("unsigned long long int", 64, true));


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
            _primitiveTypes.Add(type, Find(baseType) ?? throw new Exception($"The base type {string.Join(' ', baseType)} was not found."));
        }
    }

    public SymbolLookupTable AddTypedef(string identifier, params TokenType[] baseType)
    {
        var type = Find(baseType) ?? throw new Exception($"The base type {string.Join(' ', baseType)} was not found.");
        AddTypedef(identifier, type);
        return this;
    }

    public SymbolLookupTable AddTypedef(string identifier, Symbol symbol)
    {
        if (!_userDefinedTypes.TryAdd(identifier, symbol))
        {
            throw new InvalidOperationException($"Multiple definitions for identifier {identifier} found");
        }
        return this;
    }

    public PrimitiveTypeSymbol? Find(TokenType[] types) => _primitiveTypes.TryGetValue(types, out var type) ? type : null;
    public TypeSymbol? Find(TokenType token) => _types.TryGetValue(token, out var type) ? type : null;
    public Symbol? Find(string identifier) => _userDefinedTypes.TryGetValue(identifier, out var type) ? type : null;


    public void RegisterType(TypeSymbol type, bool overwrite = false)
    {

        if (!_userDefinedTypes.TryAdd(type.Name, type))
        {
            if (overwrite)
            {
                _userDefinedTypes[type.Name] = type;
            }
            else
            {
                throw new InvalidOperationException($"Multiple definitions for symbol {type.Name} found");
            }
        }
    }
}