using System.Linq;
using CodeGen.Syntax;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class StructParsingTests
{
    [Test]
    public void Parse_StructForwardDeclaration_ReturnStruct()
    {
        const string code = "struct TheStruct;";

        var result = new Parser(code).Parse().GetChildren().SingleOrDefault() as StructSyntaxNode;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("TheStruct"));
        Assert.That(result.Complete, Is.False);
    }

    [Test]
    public void Parse_StructWithSingleMember_ReturnStruct()
    {
        const string code = "struct TheStruct{int theField;}";

        var result = new Parser(code).Parse().GetChildren().SingleOrDefault() as StructSyntaxNode;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("TheStruct"));
        Assert.That(result.Complete, Is.True);
    }
}