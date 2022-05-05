using System.Linq;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests.Parser;

internal class ExpressionParserTests
{
    [TestCase("+")]
    [TestCase("-")]
    [TestCase("*")]
    [TestCase("/")]
    [TestCase("%")]
    [TestCase("^")]
    [TestCase("|")]
    [TestCase("&")]
    [TestCase("&&")]
    [TestCase("||")]
    public void Parse_BinaryOperator_ReturnExpression(string op)
    {
        var code = $"1 {op} 2;";

        var statement = (ExpressionStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var binary = (BinaryExpression)statement.Expression;
        var left = (LiteralExpression)binary.Left;
        var right = (LiteralExpression)binary.Right;

        Assert.That(left.Value, Is.EqualTo("1"));
        Assert.That(right.Value, Is.EqualTo("2"));
        Assert.That(binary.Operator, Is.EqualTo(op));
    }

    [TestCase("=")]
    [TestCase("+=")]
    [TestCase("-=")]
    [TestCase("*=")]
    [TestCase("/=")]
    [TestCase("^=")]
    [TestCase("|=")]
    [TestCase("&=")]
    [TestCase("!=")]
    [TestCase(">>=")]
    [TestCase("<<=")]
    public void Parse_AssigmentOperator_ReturnExpression(string op)
    {
        var code = $"a {op} 2;";

        var statement = (ExpressionStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var assignment = (AssigmentExpression)statement.Expression;
        var left = (IdentifierExpression)assignment.Left;
        var right = (LiteralExpression)assignment.Right;

        Assert.That(left.Value, Is.EqualTo("a"));
        Assert.That(right.Value, Is.EqualTo("2"));
        Assert.That(assignment.Operator, Is.EqualTo(op));
    }

    [TestCase("!")]
    [TestCase("++")]
    [TestCase("--")]
    [TestCase("~")]
    [TestCase("*")]
    [TestCase("&")]
    public void Parse_UnaryExpression_ReturnExpression(string op)
    {
        var code = $"{op}a;";

        var statement = (ExpressionStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var unary = (UnaryExpression)statement.Expression;
        var right = (IdentifierExpression)unary.Expression;

        Assert.That(right.Value, Is.EqualTo("a"));
        Assert.That(unary.Operator, Is.EqualTo(op));
    }

    [Test]
    public void Parse_ParenthesizedExpression_ReturnExpression()
    {
        var code = "(1 + 2);";

        var statement = (ExpressionStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var parenthesizedExpression = (ParenthesizedExpression)statement.Expression;
        var result = (BinaryExpression)parenthesizedExpression.Inner;
        var left = (LiteralExpression)result.Left;
        var right = (LiteralExpression)result.Right;

        Assert.That(left.Value, Is.EqualTo("1"));
        Assert.That(right.Value, Is.EqualTo("2"));
        Assert.That(result.Operator, Is.EqualTo("+"));
    }
}