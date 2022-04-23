using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class FunctionDeclarationTests
{
    [Test]
    public void Parse_NoArgumentsNoBody_ReturnFunctionDeclaration()
    {
        const string code = "void func();";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(statement.Body, Is.Null);
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(statement.Arguments, Is.Empty);
    }

    [Test]
    public void Parse_SingleArgumentNoBody_ReturnFunctionDeclaration()
    {
        const string code = "int func(long a);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument = statement.Arguments.Single();
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument.Type, Is.EqualTo("long"));
        Assert.That(argument.Name, Is.EqualTo("a"));
        Assert.That(statement.Body, Is.Null);
    }

    [Test]
    public void Parse_MultipleArgumentsNoBody_ReturnFunctionDeclaration()
    {
        const string code = "int func(long a, unsigned b);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument1 = statement.Arguments[0];
        var argument2 = statement.Arguments[1];
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument1.Type, Is.EqualTo("long"));
        Assert.That(argument1.Name, Is.EqualTo("a"));
        Assert.That(argument2.Type, Is.EqualTo("unsigned"));
        Assert.That(argument2.Name, Is.EqualTo("b"));
        Assert.That(statement.Body, Is.Null);
    }

    [Test]
    public void Parse_AnonymousArgumentNoBody_ReturnFunctionDeclaration()
    {
        const string code = "void func(void);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument = statement.Arguments[0];
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument.Type, Is.EqualTo("void"));
        Assert.That(argument.Name, Is.EqualTo(string.Empty));
        Assert.That(statement.Body, Is.Null);
    }
    [Test]
    public void Parse_MixedArgumentsNoBody_ReturnFunctionDeclaration()
    {
        const string code = "void func(void, int a);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument1 = statement.Arguments[0];
        var argument2 = statement.Arguments[1];
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument1.Type, Is.EqualTo("void"));
        Assert.That(argument1.Name, Is.EqualTo(string.Empty));
        Assert.That(argument2.Type, Is.EqualTo("int"));
        Assert.That(argument2.Name, Is.EqualTo("a"));
        Assert.That(statement.Body, Is.Null);
    }
}