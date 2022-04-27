using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class FunctionPointerParserTests
{
    [Test]
    public void Parse_SimpleFunctionPointer_ReturnFunctionDeclaration()
    {
        const string code = "typedef void (*func)()";

        var functionDeclaration = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var returnType = (BuiltInTypeExpression)functionDeclaration.ReturnType;

        Assert.That(functionDeclaration.Name, Is.EqualTo("func"));
        Assert.That(functionDeclaration.Arguments, Is.Empty);
        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(functionDeclaration.Body, Is.Null);
    }

    [Test]
    public void Parse_SimpleFunctionPointerSingleArgument_ReturnFunctionDeclaration()
    {
        const string code = "typedef void (*func)(unsigned a)";

        var functionDeclaration = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var argument = functionDeclaration.Arguments.Single();
        var variable = (IdentifierExpression)argument.Variable;
        var argumentType = (BuiltInTypeExpression)argument.Type;
        var returnType = (BuiltInTypeExpression)functionDeclaration.ReturnType;

        Assert.That(functionDeclaration.Name, Is.EqualTo("func"));
        Assert.That(variable.Value, Is.EqualTo("a"));
        Assert.That(argumentType.Types.Single(), Is.EqualTo(TokenType.Unsigned));
        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(functionDeclaration.Body, Is.Null);
    }

    [Test]
    public void Parse_ComplexReturnType_ReturnFunctionDeclaration()
    {
        const string code = "typedef const unsigned long int * (*func)()";

        var functionDeclaration = (FunctionDeclarationStatement)new Parser(code).Parse().GetChildren().Single();
        var returnType = (ConstExpression)functionDeclaration.ReturnType;
        var pointer = (PointerTypeExpression)returnType.Expression;
        var type = (BuiltInTypeExpression)pointer.Expression;

        Assert.That(functionDeclaration.Name, Is.EqualTo("func"));
        Assert.That(type.Types[0], Is.EqualTo(TokenType.Unsigned));
        Assert.That(type.Types[1], Is.EqualTo(TokenType.Long));
        Assert.That(type.Types[2], Is.EqualTo(TokenType.Int));
        Assert.That(functionDeclaration.Body, Is.Null);
    }

}