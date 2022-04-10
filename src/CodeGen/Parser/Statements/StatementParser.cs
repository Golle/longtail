using System;
using CodeGen.Parser.Expressions;
using CodeGen.Parser.Types;
using CodeGen.Tokenizer;

namespace CodeGen.Parser.Statements
{
    internal class StatementParser
    {

        public static Statement Parse(ref TokenCursor cursor, TypeLookupTable types)
        {
            return Block(ref cursor, types);
        }

        private static Statement Block(ref TokenCursor cursor, TypeLookupTable types)
        {
            if (cursor.Is(TokenType.LeftCurlyBracer))
            {
                cursor.Advance();
                var statement = Statement(ref cursor, types);
                if (cursor.Not(TokenType.RightCurlyBracer))
                {
                    throw new ParserException($"Failed to find {TokenType.RightCurlyBracer}");
                }
                cursor.Advance();
                return new BlockStatement(statement);
            }
            return Statement(ref cursor, types);
        }

        private static Statement Statement(ref TokenCursor cursor, TypeLookupTable types)
        {
            var statement = Declaration(ref cursor, types);
            if (cursor.Current.Type != TokenType.Semicolon)
            {
                throw new InvalidOperationException($"Expected ; at Line: {cursor.Current.Line} Column: {cursor.Current.Column}");
            }
            cursor.Advance();

            return statement;
        }

        private static Statement Declaration(ref TokenCursor cursor, TypeLookupTable types)
        {
            var expression = ExpressionParser.Expression(ref cursor, types);
            if (cursor.Current.Type == TokenType.Identifier)
            {
                if (expression is IdentifierExpression identifier)
                {
                    var type = types.Find(identifier.Name);
                    if (type != null)
                    {
                        return new DeclarationStatement(type, ExpressionParser.Expression(ref cursor, types));
                    }
                    throw new ParserException($"Failed to find type {identifier.Name}");
                }
                throw new ParserException($"Expected {nameof(IdentifierExpression)}");
            }
            return new ExpressionStatement(expression);
        }
    }
}
