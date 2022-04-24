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
        var argumentType = (BuiltInTypeExpression)argument.Type;
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argumentType.Types.Single(), Is.EqualTo(TokenType.Long));
        Assert.That(argument.Name, Is.EqualTo("a"));
        Assert.That(statement.Body, Is.Null);
    }

    [Test]
    public void Parse_MultipleArgumentsNoBody_ReturnFunctionDeclaration()
    {
        const string code = "int func(long a, unsigned b);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument1 = statement.Arguments[0];
        var argument1Type = (BuiltInTypeExpression)argument1.Type;
        var argument2 = statement.Arguments[1];
        var argument2Type = (BuiltInTypeExpression)argument2.Type;
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument1Type.Types.Single(), Is.EqualTo(TokenType.Long));
        Assert.That(argument1.Name, Is.EqualTo("a"));
        Assert.That(argument2Type.Types.Single(), Is.EqualTo(TokenType.Unsigned));
        Assert.That(argument2.Name, Is.EqualTo("b"));
        Assert.That(statement.Body, Is.Null);
    }

    [Test]
    public void Parse_AnonymousArgumentNoBody_ReturnFunctionDeclaration()
    {
        const string code = "void func(void);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument = statement.Arguments[0];
        var argumentType = (BuiltInTypeExpression)argument.Type;
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argumentType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(argument.Name, Is.EqualTo(string.Empty));
        Assert.That(statement.Body, Is.Null);
    }
    [Test]
    public void Parse_MixedArgumentsNoBody_ReturnFunctionDeclaration()
    {
        const string code = "void func(void, int a);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument1 = statement.Arguments[0];
        var argument1Type = (BuiltInTypeExpression)argument1.Type;
        var argument2 = statement.Arguments[1];
        var argument2Type = (BuiltInTypeExpression)argument2.Type;
        var returnType = (BuiltInTypeExpression)statement.ReturnType;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument1Type.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(argument1.Name, Is.EqualTo(string.Empty));
        Assert.That(argument2Type.Types.Single(), Is.EqualTo(TokenType.Int));
        Assert.That(argument2.Name, Is.EqualTo("a"));
        Assert.That(statement.Body, Is.Null);
    }

    [Test]
    public void Parse_ComplexArgumentType_ReturnFunctionDeclaration()
    {
        const string code = "void func(const unsigned long int * a);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var returnType = (BuiltInTypeExpression)statement.ReturnType;
        var argument = statement.Arguments.Single();
        var constArgument = (ConstExpression)argument.Type;
        var argumentPointer = (PointerTypeExpression)constArgument.Expression;
        var typeExpression = (BuiltInTypeExpression)argumentPointer.Expression;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument.Name, Is.EqualTo("a"));
        Assert.That(typeExpression.Types[0], Is.EqualTo(TokenType.Unsigned));
        Assert.That(typeExpression.Types[1], Is.EqualTo(TokenType.Long));
        Assert.That(typeExpression.Types[2], Is.EqualTo(TokenType.Int));
        Assert.That(statement.Body, Is.Null);
    }

    [Test]
    public void Parse_StructPointerArgumentType_ReturnFunctionDeclaration()
    {
        const string code = "void func(struct A * a);";

        var statement = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var returnType = (BuiltInTypeExpression)statement.ReturnType;
        var argument = statement.Arguments.Single();
        var structExpression = (StructExpression)argument.Type;
        var pointer = (PointerTypeExpression)structExpression.Expression;
        var type = (IdentifierExpression)pointer.Expression;

        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(statement.Name, Is.EqualTo("func"));
        Assert.That(argument.Name, Is.EqualTo("a"));
        Assert.That(type.Value, Is.EqualTo("A"));
        Assert.That(statement.Body, Is.Null);
    }
}