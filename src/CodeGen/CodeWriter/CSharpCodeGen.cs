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
internal record StructMember(string Name, string Type, string? Summary = null);
internal record FixedStructMember(string Name, string Type, string Initializer) : StructMember(Name, Type);

internal record FunctionCode(string Name, string ReturnType, Argument[] Arguments) : CSharpCode;
internal record Argument(string Type, string Name);

internal class CSharpCodeGen
{
    private List<EnumCode> _enums = new();

    public CSharpCode[] GenerateCode(BoundSyntaxNode[] nodes)
    {
        List<CSharpCode> _code = new();

        _enums.Clear();
        foreach (var boundSyntaxNode in nodes)
        {
            switch (boundSyntaxNode)
            {
                case BoundEnumDeclaration enumDecl:
                    var enumCode = GenerateEnumCode(enumDecl);
                    _code.Add(enumCode);
                    _enums.Add(enumCode);
                    break;
                case BoundStructDeclaration structDecl:
                    if (!structDecl.ForwardDeclaration)
                    {
                        _code.Add(GenerateStructCode(structDecl));
                    }
                    else
                    {
                        Logger.Warning($"Ignoring forward declaration for {structDecl.Type.Name}");
                    }
                    break;
                case BoundFunctionDeclaration functionDecl:
                    if (functionDecl.FunctionSymbol is not FunctionPointerSymbol)
                    {
                        _code.Add(GenerateFunctionCode(functionDecl));
                    }
                    break;
                default:
                    Logger.Error($"{boundSyntaxNode.GetType().Name} is not being handled.");
                    break;
            }
        }

        return _code.ToArray();
    }

    private CSharpCode GenerateFunctionCode(BoundFunctionDeclaration functionDecl)
    {
        var export = functionDecl.FunctionSymbol.Export;
        if (!export)
        {
            Logger.Error($"Found a function that is not marked with DllExport, not sure what to do? {functionDecl.FunctionSymbol.Name}");
        }

        var symbol = functionDecl.FunctionSymbol;
        var arguments = symbol
            .Arguments
            .Select(a => new Argument(TypeToString(a.Symbol), a.Name))
            .ToArray();

        return new FunctionCode(symbol.Name, TypeToString(symbol.ReturnType), arguments);
    }

    private CSharpCode GenerateStructCode(BoundStructDeclaration structDecl)
    {
        var members = structDecl.Type.Members
            .Select(m =>
            {
                var type = TypeToString(m.Type);
                if (m is ArrayStructMember array)
                {
                    // TODO: Find the type and bind it. (in previous classes)
                    var initializer = ExpressionToString(array.Initializer);
                    return new FixedStructMember(array.Name, type, initializer);
                }

                var summary = m.Type is FunctionPointerSymbol ? m.Type.ToString() : null;
                return new StructMember(m.Name, type, summary);
            })
            .ToArray();
        return new StructCode(structDecl.Type.Name, members);
    }

    private string ExpressionToString(BoundExpression expression)
    {
        if (expression is BoundIdentifierExpression identifier)
        {
            foreach (var enumCode in _enums)
            {
                foreach (var member in enumCode.Members)
                {
                    if (member.Name == identifier.Value)
                    {
                        return $"{enumCode.Name}.{member.Name}";
                    }
                }
            }
        }
        throw new NotSupportedException($"only {nameof(BoundIdentifierExpression)} is currently supported.");
    }



    private static string TypeToString(Symbol symbol) =>
         symbol switch
         {
             FunctionSymbol function => FunctionToDelegate(function),
             StructTypeSymbol structType => structType.Name,
             PointerTypeSymbol pointer => $"{TypeToString(pointer.Type)}*",
             PrimitiveTypeSymbol primitive => PrimitiveToString(primitive),
             TypeSymbol type => type.Name,
             ConstSymbol constSymbol => TypeToString(constSymbol.Type), // const is not supported here in c#, lets just return the containing type
             _ => throw new NotSupportedException($"{symbol.GetType().Name} is not supported in {nameof(TypeToString)}")
         };

    private static string PrimitiveToString(PrimitiveTypeSymbol primitive) =>
        (primitive.Size, primitive.Unsigned) switch
        {
            (8, true) => "byte",
            (8, false) => "sbyte",
            (16, true) => "ushort",
            (16, false) => "short",
            (32, true) => "uint",
            (64, true) => "ulong",
            (32, false) => "int",
            (64, false) => "long",
            (0, _) => "void",
            _ => throw new NotSupportedException($"{(primitive.Unsigned ? "unsigned" : "signed")} with size {primitive.Size} has not been implemented. ({primitive.Name})")
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
            builder.Append(", ");
        }
        builder.Append(TypeToString(function.ReturnType));
        builder.Append('>');
        return builder.ToString();
    }


    private static EnumCode GenerateEnumCode(BoundEnumDeclaration enumDecl)
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