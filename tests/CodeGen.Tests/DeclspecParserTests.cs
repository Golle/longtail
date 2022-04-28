using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests;

internal class DeclspecParserTests
{
    [Test]
    public void Parse_DllExport_ReturnDeclspecStatement()
    {
        const string code = "__declspec(dllexport) void func();";
        
        var statement = (DeclspecStatement)new Parser(code).Parse().GetChildren().Single();
        var function = (FunctionDeclarationStatement)statement.Expression;
        var returnType = (BuiltInTypeExpression)function.ReturnType;
        
        Assert.That(statement.Type, Is.EqualTo(TokenType.DllExport));
        Assert.That(returnType.Types.Single(), Is.EqualTo(TokenType.Void));
        Assert.That(function.Body, Is.Null);
        Assert.That(function.Name, Is.EqualTo("func"));
        Assert.That(function.Arguments, Is.Empty);
    }
}