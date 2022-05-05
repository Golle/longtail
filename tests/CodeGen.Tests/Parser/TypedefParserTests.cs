using System.Linq;
using CodeGen.Lexer;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using NUnit.Framework;

namespace CodeGen.Tests.Parser;

internal class TypedefParserTests
{

    [TestCase("uint32", TokenType.Unsigned)]
    [TestCase("uint8", TokenType.Char)]
    [TestCase("BOOL", TokenType.Bool)]
    [TestCase("uint32", TokenType.Unsigned, TokenType.Int)]
    [TestCase("uint32", TokenType.Unsigned, TokenType.Long)]
    [TestCase("uint64", TokenType.Unsigned, TokenType.Long, TokenType.Long)]
    public void Parse_TypedefBuiltInType_ReturnTypedefStatement(string name, params TokenType[] types)
    {
        var code = $"typedef {string.Join(' ', types).ToLower()} {name}";

        var typedef = (TypedefStatement)new CodeGen.Syntax.Parser(code).Parse().GetChildren().Single();
        var expressionStatement = (ExpressionStatement)typedef.Definition;
        var builtIn = (BuiltInTypeExpression)expressionStatement.Expression;

        Assert.That(typedef.Name, Is.EqualTo(name));
        for (var i = 0; i < types.Length; ++i)
        {
            Assert.That(builtIn.Types[i], Is.EqualTo(types[i]));
        }
    }


}