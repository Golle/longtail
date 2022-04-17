using System.Linq;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class BinaryOperatorPrecedenceTests
{
    [TestCase("+", "*")]
    [TestCase("+", "/")]
    [TestCase("+", "%")]
    [TestCase("-", "*")]
    [TestCase("-", "/")]
    [TestCase("-", "%")]
    [TestCase("<<", "-")]
    [TestCase("<<", "+")]
    [TestCase("<", "/")]
    [TestCase("<", "*")]
    [TestCase("&", "<")]
    [TestCase("|", ">")]
    [TestCase("&&", "&")]
    [TestCase("||", "|")]
    public void Parse_PlusAndStar_ExecuteStarFirst(string lowerPrioOp, string higherPrioOp)
    {
        var code = $"1 {lowerPrioOp} 2 {higherPrioOp} 3";

        var binary1 = (BinaryExpression)new Parser(code).Parse().GetChildren().Single();
        var left1 = (LiteralExpression)binary1.Left;
        var binary2 = (BinaryExpression)binary1.Right;
        var left2 = (LiteralExpression)binary2.Left;
        var right2 = (LiteralExpression)binary2.Right;

        Assert.That(binary1.Operator, Is.EqualTo(lowerPrioOp));
        Assert.That(left1.Value, Is.EqualTo("1"));
        Assert.That(binary2.Operator, Is.EqualTo(higherPrioOp));
        Assert.That(left2.Value, Is.EqualTo("2"));
        Assert.That(right2.Value, Is.EqualTo("3"));
    }
}