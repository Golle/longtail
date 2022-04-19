using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class VariableDeclarationTests
{
    [Test]
    public void Parse_EmptyDecklaration_ReturnStatement()
    {
        var code = "int a;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();

        Assert.That(statement.Type, Is.EqualTo("int"));
        Assert.That(statement.Identifier, Is.EqualTo("a"));
        Assert.That(statement.AssignmentExpression, Is.Null);
    }

    [Test]
    public void Parse_AssignSingleValue_ReturnStatement()
    {
        var code = "int a = 1;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var literal = (LiteralExpression)statement.AssignmentExpression!;

        Assert.That(statement.Type, Is.EqualTo("int"));
        Assert.That(statement.Identifier, Is.EqualTo("a"));
        Assert.That(literal.Value, Is.EqualTo("1"));
        Assert.That(literal.Type, Is.EqualTo(TokenType.Number));
    }

    [Test]
    public void Parse_AssignBinaryExpressionValue_ReturnStatement()
    {
        var code = "int a = b + 2;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var binary = (BinaryExpression)statement.AssignmentExpression!;
        var left = (IdentifierExpression)binary.Left;
        var right = (LiteralExpression)binary.Right;

        Assert.That(statement.Type, Is.EqualTo("int"));
        Assert.That(statement.Identifier, Is.EqualTo("a"));
        Assert.That(binary.Operator, Is.EqualTo("+"));
        Assert.That(left.Value, Is.EqualTo("b"));
        Assert.That(right.Type, Is.EqualTo(TokenType.Number));
        Assert.That(right.Value, Is.EqualTo("2"));
    }
}