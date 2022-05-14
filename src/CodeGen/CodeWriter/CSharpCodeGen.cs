using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGen.Logging;
using CodeGen.Syntax.Binding;
using CodeGen.Syntax.Symbols;

namespace CodeGen.CodeWriter;

internal record CSharpCode;

internal record EnumCode(string Name, EnumMember[] Members) : CSharpCode;
internal record EnumMember(string Name, string? Value = null);

internal record StructCode(string Name, StructMember[] Members) : CSharpCode;
internal record StructMember(string Name, string Type);


internal class CSharpCodeGen
{
    public CSharpCodeGen()
    {
    }

    public CSharpCode[] GenerateCode(BoundSyntaxNode[] nodes)
    {
        List<CSharpCode> _code = new();

        foreach (var boundSyntaxNode in nodes)
        {
            switch (boundSyntaxNode)
            {
                case BoundEnumDeclaration enumDecl:
                    _code.Add(GenerateEnumCode(enumDecl));
                    break;
                case BoundStructDeclaration structDecl:
                    if (!structDecl.ForwardDeclaration)
                    {
                        Logger.Warning($"Ignoring forward declaration for {structDecl.Type.Name}");
                        _code.Add(GenerateStructCode(structDecl));
                    }
                    break;
            }
        }

        return _code.ToArray();
    }

    private static CSharpCode GenerateStructCode(BoundStructDeclaration structDecl)
    {
        var members = structDecl.Type.Members
            .Select(m =>
            {
                var type = TypeToString(m.Type);
                return new StructMember(m.Name, type);
            })
            .ToArray();
        return new StructCode(structDecl.Type.Name, members);
    }

    //private static string TypeToString(Symbol symbol)
    //{
    //    var value = symbol switch
    //    {
    //        FunctionSymbol function => FunctionToDelegate(function),
    //        StructTypeSymbol structType => structType.Name,
    //        ConstSymbol constSymbol => TypeToString(constSymbol.Type),
    //        _ => throw new NotSupportedException($"{symbol.GetType().Name} has not been implemented as a valid member type.")
    //    };
    //}

    private static string TypeToString(Symbol symbol) =>
        symbol switch
        {
            FunctionSymbol function => FunctionToDelegate(function),
            StructTypeSymbol structType => structType.Name,
            PointerTypeSymbol pointer => $"{TypeToString(pointer.Type)}*",
            TypeSymbol type => type.Name,
            ConstSymbol constSymbol => TypeToString(constSymbol.Type), // const is not supported here i c#, lets just return the containing type
            _ => throw new NotSupportedException($"{symbol.GetType().Name} is not supported in {nameof(TypeToString)}")
        };

    private static string FunctionToDelegate(FunctionSymbol function)
    {
        const string callConv = "Cdecl";
        var builder = new StringBuilder();
        builder.Append($"delegate* unmanaged[{callConv}]");
        builder.Append('<');
        foreach (var functionArgument in function.Arguments)
        {
            builder.Append(TypeToString(functionArgument.Symbol));
            builder.Append(',');
        }
        builder.Append(TypeToString(function.ReturnType));
        builder.Append('>');
        return builder.ToString();
    }

    private static CSharpCode GenerateEnumCode(BoundEnumDeclaration enumDecl)
    {
        var members = enumDecl.Members
            .Select(m => m switch
            {
                BoundIdentifierExpression identifier => new EnumMember(identifier.Value),
                BoundAssignmentExpression assignment => new EnumMember(ExpressionToValue(assignment.Left), ExpressionToValue(assignment.Right)),
                _ => throw new NotImplementedException($"enum support for {m.GetType().Name} has not been implemented yet.")
            })
            .ToArray();

        var name = enumDecl.Type.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            // guess  the name by reading all member names and using whatever is common between them
            name = GenerateEnumName(members);
        }

        return new EnumCode(name, members);

        static string ExpressionToValue(BoundExpression expression)
        {
            if (expression is BoundIdentifierExpression identifier)
            {
                return identifier.Value;
            }

            if (expression is BoundLiteralExpression literal)
            {
                return literal.Value;
            }
            throw new NotSupportedException($"{expression.GetType().Name} is not supported in enum");
        }

        static string GenerateEnumName(EnumMember[] members)
        {
            if (members.Length == 0)
            {
                throw new InvalidOperationException("No enum members, can't guess the name of the enum.");
            }

            const int maxLength = 128;
            Span<char> buff = stackalloc char[maxLength];
            members[0].Name.CopyTo(buff);
            var charCount = members[0].Name.Length;
            for (var i = 1; i < members.Length; ++i)
            {
                var (name, _) = members[i];
                for (var j = 0; j < name.Length; ++j)
                {
                    if (name[j] != buff[j])
                    {
                        charCount = j;
                        break;
                    }
                }
            }
            if (!char.IsLetterOrDigit(buff[charCount - 1]))
            {
                charCount--;
            }
            if (charCount <= 0)
            {
                throw new InvalidOperationException("Failed to guess the name of the enum.");
            }

            //NOTE(Jens): Prevent name conflicts (not the best solution,maybe we should use const fields in the library for these.
            "_Enum".CopyTo(buff.Slice(charCount));
            charCount += 5;
            return new string(buff.Slice(0, charCount));
        }
    }
}