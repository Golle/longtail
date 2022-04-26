using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests;

//[Ignore("Not implemented yet")]
internal class StructParsingTests
{
    [Test]
    public void Parse_StructForwardDeclaration_ReturnStruct()
    {
        const string code = "struct TheStruct;";

        var result = (StructDeclarationStatement)new Parser(code).Parse().GetChildren().Single();

        Assert.That(result.ForwardDeclaration, Is.True);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(result.Members, Is.Empty);
    }

    [Test]
    public void Parse_StructWithSingleMember_ReturnStruct()
    {
        const string code = "struct TheStruct{int theField;}";

        var result = (StructDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var member = result.Members.Single();
        var memberType = (BuiltInTypeExpression)member.Type;

        Assert.That(result.ForwardDeclaration, Is.False);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(member.Name, Is.EqualTo("theField"));
        Assert.That(memberType.Types.Single(), Is.EqualTo(TokenType.Int));
    }


    [Test]
    public void Parse_StructWithMultipleMembers_ReturnStruct()
    {
        const string code = "struct TheStruct{int first; unsigned second; }";

        var result = (StructDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var member1 = result.Members[0];
        var memberType1 = (BuiltInTypeExpression)member1.Type;
        var member2 = result.Members[1];
        var memberType2 = (BuiltInTypeExpression)member2.Type;

        Assert.That(result.ForwardDeclaration, Is.False);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(member1.Name, Is.EqualTo("first"));
        Assert.That(memberType1.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(member2.Name, Is.EqualTo("second"));
        Assert.That(memberType2.Types.Single(), Is.EqualTo(TokenType.Unsigned));
    }

    [Test]
    public void Parse_StructWithSinglePointerMember_ReturnStruct()
    {
        const string code = "struct TheStruct{unsigned int* thePointer;}";

        var result = (StructDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var member = result.Members.Single();
        var pointerType = (PointerTypeExpression)member.Type;
        var memberType = (BuiltInTypeExpression)pointerType.Expression;


        Assert.That(result.ForwardDeclaration, Is.False);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(member.Name, Is.EqualTo("thePointer"));
        Assert.That(memberType.Types[0], Is.EqualTo(TokenType.Unsigned));
        Assert.That(memberType.Types[1], Is.EqualTo(TokenType.Int));
    }
}