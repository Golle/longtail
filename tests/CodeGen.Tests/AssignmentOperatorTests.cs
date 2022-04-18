using System.Linq;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class AssignmentOperatorTests
{
    [Test]
    public void Parse_SimpleAssignment_ReturnExpression()
    {
        const string code = "a = 1;";

        var statement = (ExpressionStatement)new Parser(code).Parse().GetChildren().Single();
        var binary = (AssigmentExpression)statement.Expression;
        var left = (IdentifierExpression)binary.Left;
        var right = (LiteralExpression)binary.Right;

        Assert.That(binary.Operator, Is.EqualTo("="));
        Assert.That(left.Value, Is.EqualTo("a"));
        Assert.That(right.Value, Is.EqualTo("1"));
    }


    [TestCase("+=")]
    [TestCase("-=")]
    [TestCase("*=")]
    [TestCase("/=")]
    [TestCase("^=")]
    [TestCase(">>=")]
    [TestCase("<<=")]
    [TestCase("%=")]
    [TestCase("&=")]
    [TestCase("|=")]
    public void Parse_CompoundAssignment_ReturnExpression(string op)
    {
        var code = $"a {op} 1;";

        var statement = (ExpressionStatement)new Parser(code).Parse().GetChildren().Single();
        var binary = (AssigmentExpression)statement.Expression;
        var left = (IdentifierExpression)binary.Left;
        var right = (LiteralExpression)binary.Right;

        Assert.That(binary.Operator, Is.EqualTo(op));
        Assert.That(left.Value, Is.EqualTo("a"));
        Assert.That(right.Value, Is.EqualTo("1"));
    }
}