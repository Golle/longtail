using System;
using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax.Binding;
using NUnit.Framework;

namespace CodeGen.Tests.Syntax;

internal class BuiltInTypeComparerTests
{

    [Test]
    public void Equals_BothNull_ReturnTrue()
    {
        var result = new BuiltInTypeComparer().Equals(null, null);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_LeftIsNull_ReturnFalse()
    {
        var result = new BuiltInTypeComparer().Equals(null, Array.Empty<TokenType>());

        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_RightIsNull_ReturnFalse()
    {
        var result = new BuiltInTypeComparer().Equals(Array.Empty<TokenType>(), null);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_DifferentLength_ReturnFalse()
    {
        var result = new BuiltInTypeComparer().Equals(new TokenType[1], new TokenType[2]);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_MatchOnSingleType_ReturnTrue()
    {
        var result = new BuiltInTypeComparer().Equals(new[] { TokenType.Unsigned }, new[] { TokenType.Unsigned });

        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_NoMatchOnSingleType_ReturnFalse()
    {
        var result = new BuiltInTypeComparer().Equals(new[] { TokenType.Int }, new[] { TokenType.Unsigned });

        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_MatchOnMultipleTypes_ReturnTrue()
    {
        var result = new BuiltInTypeComparer().Equals(new[] { TokenType.Unsigned, TokenType.Int }, new[] { TokenType.Unsigned, TokenType.Int });

        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_NoMatchOnMultipleTypes_ReturnFalse()
    {
        var result = new BuiltInTypeComparer().Equals(new[] { TokenType.Unsigned, TokenType.Long }, new[] { TokenType.Unsigned, TokenType.Int });

        Assert.That(result, Is.False);
    }







    [TestCase(TokenType.Unsigned)]
    [TestCase(TokenType.Int)]
    [TestCase(TokenType.Long)]
    [TestCase(TokenType.Void)]
    public void GetHashCode_SingleElement_ReturnCode(TokenType type)
    {
        var result = new BuiltInTypeComparer().GetHashCode(new[] { type });

        Assert.That(result, Is.EqualTo((int)type));
    }

    [TestCase(TokenType.Int, TokenType.Unsigned)]
    [TestCase(TokenType.Int, TokenType.Void, TokenType.Unsigned)]
    [TestCase(TokenType.Int, TokenType.Char)]
    public void GetHashCode_MultipleElements_ReturnCode(params TokenType[] types)
    {
        var expected = types.Sum(t => (int)t);
        var result = new BuiltInTypeComparer().GetHashCode(types);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetHashCode_MultipleElementsDiffernetOrder_ReturnSameCode()
    {
        var code1 = new BuiltInTypeComparer().GetHashCode(new[] { TokenType.Int, TokenType.Unsigned });
        var code2 = new BuiltInTypeComparer().GetHashCode(new[] { TokenType.Unsigned, TokenType.Int });

        Assert.That(code1, Is.EqualTo(code2));
    }

}