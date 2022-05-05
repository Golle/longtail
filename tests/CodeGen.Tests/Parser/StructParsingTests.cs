using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests.Parser;

internal class StructParsingTests
{
    [Test]
    public void Parse_StructForwardDeclaration_ReturnStruct()
    {
        const string code = "struct TheStruct;";

        var result = (StructDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();

        Assert.That(result.ForwardDeclaration, Is.True);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(result.Members, Is.Empty);
    }

    [Test]
    public void Parse_StructWithSingleMember_ReturnStruct()
    {
        const string code = "struct TheStruct{int theField;}";

        var result = (StructDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var member = result.Members.Single();
        var variable = (IdentifierExpression)member.Variable;
        var memberType = (BuiltInTypeExpression)member.Type;

        Assert.That(result.ForwardDeclaration, Is.False);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(variable.Value, Is.EqualTo("theField"));
        Assert.That(memberType.Types.Single(), Is.EqualTo(TokenType.Int));
    }


    [Test]
    public void Parse_StructWithMultipleMembers_ReturnStruct()
    {
        const string code = "struct TheStruct{int first; unsigned second; }";

        var result = (StructDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var member1 = result.Members[0];
        var variable1 = (IdentifierExpression)member1.Variable;
        var memberType1 = (BuiltInTypeExpression)member1.Type;
        var member2 = result.Members[1];
        var variable2 = (IdentifierExpression)member2.Variable;
        var memberType2 = (BuiltInTypeExpression)member2.Type;

        Assert.That(result.ForwardDeclaration, Is.False);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(variable1.Value, Is.EqualTo("first"));
        Assert.That(memberType1.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(variable2.Value, Is.EqualTo("second"));
        Assert.That(memberType2.Types.Single(), Is.EqualTo(TokenType.Unsigned));
    }

    [Test]
    public void Parse_StructWithSinglePointerMember_ReturnStruct()
    {
        const string code = "struct TheStruct{unsigned int* thePointer;}";

        var result = (StructDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var member = result.Members.Single();
        var variable = (IdentifierExpression)member.Variable;
        var pointerType = (PointerTypeExpression)member.Type;
        var memberType = (BuiltInTypeExpression)pointerType.Expression;


        Assert.That(result.ForwardDeclaration, Is.False);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(variable.Value, Is.EqualTo("thePointer"));
        Assert.That(memberType.Types[0], Is.EqualTo(TokenType.Unsigned));
        Assert.That(memberType.Types[1], Is.EqualTo(TokenType.Int));
    }

    [Test]
    public void Parse_StructWithSingleArrayMember_ReturnStruct()
    {
        const string code = "struct TheStruct{unsigned theArray[1];}";

        var result = (StructDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var member = result.Members.Single();
        var array = (ArrayExpression)member.Variable;
        var variable = (IdentifierExpression)array.Left;
        var accessor = (LiteralExpression)array.Expression!;
        var memberType = (BuiltInTypeExpression)member.Type;


        Assert.That(result.ForwardDeclaration, Is.False);
        Assert.That(result.Name, Is.EqualTo("TheStruct"));
        Assert.That(variable.Value, Is.EqualTo("theArray"));
        Assert.That(accessor.Value, Is.EqualTo("1"));
        Assert.That(accessor.Type, Is.EqualTo(TokenType.Number));
        Assert.That(memberType.Types.Single(), Is.EqualTo(TokenType.Unsigned));
    }
}