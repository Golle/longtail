using System;
using CodeGen.Parser;
using CodeGen.Tokenizer;
using NUnit.Framework;

namespace CodeGen.Tests.Parser;

public class TokenCursorTests
{
    private readonly Token[] _tokens;
    public TokenCursorTests()
    {
        _tokens = new[]
        {
            new Token { Type = TokenType.PreProcessor, Value = "a" },
            new Token { Type = TokenType.Identifier, Value = "b" },
            new Token { Type = TokenType.NewLine, Value = "" },
            new Token { Type = TokenType.NewLine, Value = "" },
            new Token { Type = TokenType.NewLine, Value = "" },
            new Token { Type = TokenType.Boolean, Value = "" },
            new Token { Type = TokenType.Identifier, Value = "c" }
        };
    }

    [Test]
    public void Ctor_Always_ReturnFirstToken()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Current;

        Assert.That(result.Type, Is.EqualTo(TokenType.PreProcessor));
        Assert.That(result.Value, Is.EqualTo("a"));
    }

    [Test]
    public void Advance_Always_MoveOneStep()
    {
        var cursor = new TokenCursor(_tokens);

        cursor.Advance();

        Assert.That(cursor.Current.Type, Is.EqualTo(TokenType.Identifier));
    }

    [Test]
    public void Advance_SkipNewLine_MoveToTokenAfterNewLine()
    {
        var cursor = new TokenCursor(_tokens);

        cursor.Advance(2, skipNewLine: true);

        Assert.That(cursor.Current.Type, Is.EqualTo(TokenType.Boolean));
    }

    [Test]
    public void Advance_DontSkipNewLine_MoveToNewLine()
    {
        var cursor = new TokenCursor(_tokens);

        cursor.Advance(2, skipNewLine: false);

        Assert.That(cursor.Current.Type, Is.EqualTo(TokenType.NewLine));
    }

    [Test]
    public void Advance_OutOfBounds_ReturnFalse()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Advance((uint)_tokens.Length, skipNewLine: false);

        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Advance_WithinBounds_ReturnTrue()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Advance((uint)_tokens.Length-1, skipNewLine: false);

        Assert.That(result, Is.True);
    }


    [Test]
    public void Peek_Zero_ReturnCurrent()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Peek(0);

        Assert.That(result.Type, Is.EqualTo(TokenType.PreProcessor));
    } 
    
    [Test]
    public void Peek_One_ReturnNext()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Peek(1);

        Assert.That(result.Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(result.Value, Is.EqualTo("b"));
    }

    [Test]
    public void Peek_DontSkipNewLine_ReturnNewLine()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Peek(2, skipNewLine: false);

        Assert.That(result.Type, Is.EqualTo(TokenType.NewLine));
    } 
    
    [Test]
    public void Peek_SkipNewLine_ReturnNext()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Peek(2, skipNewLine: true);

        Assert.That(result.Type, Is.EqualTo(TokenType.Boolean));
    }

    [Test]
    public void Peek_ToTheLastToken_ReturnLastToken()
    {
        var cursor = new TokenCursor(_tokens);

        var result = cursor.Peek(_tokens.Length - 1, skipNewLine: false);
        
        Assert.That(result.Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(result.Value, Is.EqualTo("c"));
    }

    [Test]
    public void Peek_PastTheLastLine_ThrowException()
    {
        var result = Assert.Catch<IndexOutOfRangeException>(() => new TokenCursor(_tokens).Peek(_tokens.Length, skipNewLine: false));

        Assert.That(result, Is.Not.Null);
    }
}