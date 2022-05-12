using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.Syntax.Binding;

namespace CodeGen.CodeWriter;

internal record CSharpCode;

internal record EnumCode(string Name, EnumMember[] Members) : CSharpCode;
internal record EnumMember(string Name, string? Value = null);

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
            }
        }

        return _code.ToArray();
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
            if (!char.IsLetterOrDigit(buff[charCount-1]))
            {
                charCount--;
            }
            if (charCount <= 0)
            {
                throw new InvalidOperationException("Failed to guess the name of the enum.");
            }
            return new string(buff.Slice(0, charCount));
        }
    }
}