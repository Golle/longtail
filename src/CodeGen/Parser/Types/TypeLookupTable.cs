using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.Parser.Types;
internal class TypeLookupTable
{
    private readonly List<TypeDeclaration> _types = new();
    private readonly List<TypedefDeclaration> _typedefs = new();

    public TypeDeclaration? Find(string name) =>
        _types.FirstOrDefault(t => t.Name == name)
        ?? _typedefs.FirstOrDefault(t => t.Name == name);

    public TypeDeclaration Get(string name) => Find(name) ?? throw new Exception($"Type {name} could not be found.");
    public TypeLookupTable AddTypedef(string name, string parent, bool pointer = false) => AddTypedef(name, Get(parent), pointer);
    public TypeLookupTable AddTypedef(string name, TypeDeclaration parent, bool pointer = false)
    {
        if (_typedefs.Any(t => t.Name == name))
        {
            throw new Exception($"Multiple definitions of {name}");
        }
        _typedefs.Add(new TypedefDeclaration(name, parent, pointer));
        return this;
    }

    public TypeLookupTable Add(TypeDeclaration type)
    {
        if (_types.Any(t => t.Name == type.Name))
        {
            throw new Exception($"Multiple definitions with name {type.Name}");
        }
        _types.Add(type);
        return this;
    }

    private TypeLookupTable(IEnumerable<TypeDeclaration> baseTypes)
    {
        _types.AddRange(baseTypes);
    }

    public static TypeLookupTable CreateDefault()
    {
        var types = new TypeLookupTable(new PrimitiveTypeDeclaration[]
        {
            new("short int"),
            new("unsigned short int"),
            new("int"),
            new("unsigned int"),
            new("long int"),
            new("unsigned long int"),
            new("long long int"),
            new("unsigned long long int"),
            new("void"),
            new("bool"),
            new("char"),
            new("unsigned char"),
            new("float"),
            new("double")
        });

        // treat all variations of primitive types as type defs, will make it easier to translate them later
        types.AddTypedef("short", "short int")
            .AddTypedef("signed short", "short int")
            .AddTypedef("signed short int", "short int")
            .AddTypedef("unsigned short", "unsigned short int")
            .AddTypedef("signed", "int")
            .AddTypedef("signed int", "int")
            .AddTypedef("unsigned", "unsigned int")
            .AddTypedef("long", "long int")
            .AddTypedef("signed long", "long int")
            .AddTypedef("signed long int", "long int")
            .AddTypedef("unsigned long", "unsigned long int")
            .AddTypedef("long long", "long long int")
            .AddTypedef("signed long long", "long long int")
            .AddTypedef("signed long long int", "long long int")
            .AddTypedef("unsigned long long", "unsigned long long int");

        return types;
    }
}
