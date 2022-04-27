using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class ArrayDeclarationTests
{
    [Test]
    public void Parse_SimpleArrayDeclaration_ReturnStatement()
    {
        const string code = "int a[1];";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var type = (BuiltInTypeExpression)statement.Type;
        var array = (ArrayExpression)statement.Variable;
        var variable = (IdentifierExpression)array.Left;
        var accessor = (LiteralExpression)array.Expression;

        Assert.That(type.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(variable.Value, Is.EqualTo("a"));
        Assert.That(accessor.Value, Is.EqualTo("1"));
        Assert.That(accessor.Type, Is.EqualTo(TokenType.Number));
    }

    [Test]
    public void Parse_ComplexTypeArrayDeclaration_ReturnStatement()
    {
        const string code = "unsigned int * a[1];";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var pointer = (PointerTypeExpression)statement.Type;
        var type = (BuiltInTypeExpression)pointer.Expression;
        var array = (ArrayExpression)statement.Variable;
        var variable = (IdentifierExpression)array.Left;
        var accessor = (LiteralExpression)array.Expression;

        Assert.That(type.Types[0], Is.EqualTo(TokenType.Unsigned));
        Assert.That(type.Types[1], Is.EqualTo(TokenType.Int));
        Assert.That(variable.Value, Is.EqualTo("a"));
        Assert.That(accessor.Value, Is.EqualTo("1"));
        Assert.That(accessor.Type, Is.EqualTo(TokenType.Number));
    }

    [Test]
    public void Parse_ComplexAccessorArrayDeclaration_ReturnStatement()
    {
        const string code = "unsigned a[(1+2)];";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var type = (BuiltInTypeExpression)statement.Type;
        var array = (ArrayExpression)statement.Variable;
        var variable = (IdentifierExpression)array.Left;
        var group = (ParenthesizedExpression)array.Expression;
        var inner = (BinaryExpression)group.Inner;
        var literal1 = (LiteralExpression)inner.Left;
        var literal2 = (LiteralExpression)inner.Right;
        
        Assert.That(type.Types.Single(), Is.EqualTo(TokenType.Unsigned));
        Assert.That(variable.Value, Is.EqualTo("a"));
        Assert.That(literal1.Value, Is.EqualTo("1"));
        Assert.That(literal1.Type, Is.EqualTo(TokenType.Number));
        Assert.That(literal2.Value, Is.EqualTo("2"));
        Assert.That(literal2.Type, Is.EqualTo(TokenType.Number));
    }
}