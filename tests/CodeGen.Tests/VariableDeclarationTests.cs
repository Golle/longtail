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
    public void Parse_EmptyDeclaration_ReturnStatement()
    {
        var code = "int a;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var type = (BuiltInTypeExpression)statement.Type;
        var variable = (IdentifierExpression)statement.Variable;

        Assert.That(type.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(variable.Value, Is.EqualTo("a"));
    }

    [Test]
    public void Parse_AdvancedTypeDeclaration_ReturnStatement()
    {
        var code = "unsigned long int ** a;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var pointer1 = (PointerTypeExpression)statement.Type;
        var pointer2 = (PointerTypeExpression)pointer1.Expression;
        var type = (BuiltInTypeExpression)pointer2.Expression;
        var variable = (IdentifierExpression)statement.Variable;

        Assert.That(type.Types[0], Is.EqualTo(TokenType.Unsigned));
        Assert.That(type.Types[1], Is.EqualTo(TokenType.Long));
        Assert.That(type.Types[2], Is.EqualTo(TokenType.Int));
        Assert.That(variable.Value, Is.EqualTo("a"));
    }

    [Test]
    public void Parse_AdvancedTypeDeclarationAssignment_ReturnStatement()
    {
        var code = "unsigned int * a = 1;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var assignment = (AssigmentExpression)statement.Variable;
        var literal = (LiteralExpression)assignment.Right;
        var identifier = (IdentifierExpression)assignment.Left;
        var pointer = (PointerTypeExpression)statement.Type;
        var type = (BuiltInTypeExpression)pointer.Expression;

        Assert.That(type.Types[0], Is.EqualTo(TokenType.Unsigned));
        Assert.That(type.Types[1], Is.EqualTo(TokenType.Int));
        Assert.That(identifier.Value, Is.EqualTo("a"));
        Assert.That(literal.Type, Is.EqualTo(TokenType.Number));
        Assert.That(literal.Value, Is.EqualTo("1"));
    }

    [Test]
    public void Parse_AssignSingleValue_ReturnStatement()
    {
        var code = "int a = 1;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var assignment = (AssigmentExpression)statement.Variable;
        var literal = (LiteralExpression)assignment.Right;
        var identifier = (IdentifierExpression)assignment.Left;
        var type = (BuiltInTypeExpression)statement.Type;

        Assert.That(type.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(identifier.Value, Is.EqualTo("a"));
        Assert.That(literal.Value, Is.EqualTo("1"));
        Assert.That(literal.Type, Is.EqualTo(TokenType.Number));
    }

    [Test]
    public void Parse_AssignBinaryExpressionValue_ReturnStatement()
    {
        var code = "int a = b + 2;";

        var statement = (VariableDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var assignment = (AssigmentExpression)statement.Variable;
        var binary = (BinaryExpression)assignment.Right;
        var variable = (IdentifierExpression)assignment.Left;
        var left = (IdentifierExpression)binary.Left;
        var right = (LiteralExpression)binary.Right;
        var type = (BuiltInTypeExpression)statement.Type;

        Assert.That(type.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(variable.Value, Is.EqualTo("a"));
        Assert.That(binary.Operator, Is.EqualTo("+"));
        Assert.That(left.Value, Is.EqualTo("b"));
        Assert.That(right.Type, Is.EqualTo(TokenType.Number));
        Assert.That(right.Value, Is.EqualTo("2"));
    }


}