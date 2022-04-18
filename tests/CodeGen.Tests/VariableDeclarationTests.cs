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
}