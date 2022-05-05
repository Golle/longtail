using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests.Parser;

internal class EnumParserTests
{
    [Test]
    public void Parse_EmptyEnum_ReturnEnumDeclarationStatement()
    {
        const string code = "enum TheEnum{};";

        var enumDeclaration = (EnumDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();

        Assert.That(enumDeclaration.Name, Is.EqualTo("TheEnum"));
        Assert.That(enumDeclaration.Members, Is.Empty);
    }

    [Test]
    public void Parse_EnumWithSingleMember_ReturnEnumDeclarationStatement()
    {
        const string code = "enum TheEnum{ Value };";

        var enumDeclaration = (EnumDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var member = (IdentifierExpression)enumDeclaration.Members.Single();
        
        Assert.That(enumDeclaration.Name, Is.EqualTo("TheEnum"));
        Assert.That(member.Value, Is.EqualTo("Value"));
    }


    [Test]
    public void Parse_AnonymousEnumWithSingleMember_ReturnEnumDeclarationStatement()
    {
        const string code = "enum { Value };";

        var enumDeclaration = (EnumDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var member = (IdentifierExpression)enumDeclaration.Members.Single();

        Assert.That(enumDeclaration.Name, Is.EqualTo(string.Empty));
        Assert.That(member.Value, Is.EqualTo("Value"));
    }

    [Test]
    public void Parse_EnumWithMultipleMembers_ReturnEnumDeclarationStatement()
    {
        const string code = "enum TheEnum{ Value1, Value2 };";

        var enumDeclaration = (EnumDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var member1 = (IdentifierExpression)enumDeclaration.Members[0];
        var member2 = (IdentifierExpression)enumDeclaration.Members[1];

        Assert.That(enumDeclaration.Name, Is.EqualTo("TheEnum"));
        Assert.That(member1.Value, Is.EqualTo("Value1"));
        Assert.That(member2.Value, Is.EqualTo("Value2"));
    }

    [Test]
    public void Parse_EnumWithExpressionMember_ReturnEnumDeclarationStatement()
    {
        const string code = "enum TheEnum{ Value = 1 };";

        var enumDeclaration = (EnumDeclarationStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var assignment = (AssigmentExpression)enumDeclaration.Members.Single();
        var member = (IdentifierExpression)assignment.Left;
        var literal = (LiteralExpression)assignment.Right;

        Assert.That(enumDeclaration.Name, Is.EqualTo("TheEnum"));
        Assert.That(assignment.Operator, Is.EqualTo("="));
        Assert.That(member.Value, Is.EqualTo("Value"));
        Assert.That(literal.Type, Is.EqualTo(TokenType.Number));
        Assert.That(literal.Value, Is.EqualTo("1"));
    }
}