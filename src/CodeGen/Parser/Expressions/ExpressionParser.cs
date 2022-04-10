using System;
using System.Collections.Generic;
using System.Text;
using CodeGen.Parser.Types;
using CodeGen.Tokenizer;

namespace CodeGen.Parser.Expressions;

internal class ExpressionParser
{
    /* TODO: this is not updated
     expression     array;

    array           equality[(expression)];
    equality        bitwise ( ( "!=" | "==" ) bitwise )* ;
    bitwise         comparison ( ( "&" | "|" ) comparison )*
    comparison      shift ( ( ">" | ">=" | "<" | "<=" ) shift )* ;
    shift           term ( ( "<<" | ">>" ) term )* ;
    term            factor ( ( "-" | "+" ) factor )* ;
    factor          unary ( ( "/" | "*" | "%" ) unary )* ;
    unary           ( "!" | "-" ) unary
                   | primary ;
    primary         NUMBER | STRING | "true" | "false" | "null"
               | "(" expression ")" ;
     *
     */


    public static Expression Expression(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Assignment(ref cursor, types);

        return expression;

    }
    public static Expression Assignment(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Accessors(ref cursor, types);
        if (cursor.Is(TokenType.Equal))
        {
            cursor.Advance();
            expression = new AssignmentExpression(expression, Expression(ref cursor, types));
        }

        return expression;
    }
    public static Expression Accessors(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Equality(ref cursor, types);

        while (true)
        {
            switch (cursor.Current.Type)
            {
                case TokenType.LeftParenthesis:
                    expression = ParseFunctionCall(expression, ref cursor, types);
                    break;
                case TokenType.LeftSquareBracket:
                    expression = ParseArray(expression, ref cursor, types);
                    break;
                case TokenType.Punctuation:
                    cursor.Advance();
                    expression = new AccessorExpression(expression, Equality(ref cursor, types), ".");
                    break;
                case TokenType.Pointer:
                    cursor.Advance();
                    expression = new AccessorExpression(expression, Equality(ref cursor, types), "->");
                    break;

                default:
                    return expression;
            }
        }

        static Expression ParseFunctionCall(Expression expression, ref TokenCursor cursor, TypeLookupTable types)
        {
            cursor.Advance();

            List<Expression> arguments = new();
            if (cursor.Current.Type != TokenType.RightParenthesis)
            {
                while (true)
                {
                    var item = Expression(ref cursor, types);
                    arguments.Add(item);
                    if (cursor.Current.Type == TokenType.Comma)
                    {
                        cursor.Advance();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            var function = new FunctionExpression(expression, arguments);
            if (cursor.Current.Type != TokenType.RightParenthesis)
            {
                throw new Exception($"Expected {TokenType.RightParenthesis} at Line: {cursor.Current.Line} Column: {cursor.Current.Column}");
            }
            cursor.Advance();
            return function;
        }

        static Expression ParseArray(Expression expression, ref TokenCursor cursor, TypeLookupTable types)
        {
            cursor.Advance();
            var array = new ArrayExpression(expression, Expression(ref cursor, types));
            if (cursor.Current.Type != TokenType.RightSquareBracket)
            {
                throw new Exception($"Failed to find {TokenType.RightSquareBracket}");
            }
            cursor.Advance();
            return array;
        }
    }

    private static Expression Equality(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Bitwise(ref cursor, types);

        while (cursor.Is(TokenType.CompoundOperator) && cursor.Current.Value is "==" or "!=" or "&&" or "||")
        {
            var op = cursor.Current.Value;
            cursor.Advance();
            expression = new OperatorExpression(expression, Bitwise(ref cursor, types), op);
        }

        return expression;
    }

    private static Expression Bitwise(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Comparision(ref cursor, types);

        while (cursor.Is(TokenType.Operator) && cursor.Current.Value is "&" or "|" or "^")
        {
            var op = cursor.Current.Value;
            cursor.Advance();
            expression = new OperatorExpression(expression, Comparision(ref cursor, types), op);
        }

        return expression;
    }

    private static Expression Comparision(ref TokenCursor cursor, TypeLookupTable types)
    {
        static bool IsComparisonOperator(in Token token) =>
            (token.Type is TokenType.Operator && token.Value is "<" or ">") ||
            (token.Type is TokenType.CompoundOperator && token.Value is "<=" or ">=");

        var expression = Shifts(ref cursor, types);
        while (IsComparisonOperator(cursor.Current))
        {
            var op = cursor.Current.Value;
            cursor.Advance();
            expression = new OperatorExpression(expression, Shifts(ref cursor, types), op);
        }
        return expression;
    }

    private static Expression Shifts(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Term(ref cursor, types);

        while (cursor.Current.Type is TokenType.CompoundOperator && cursor.Current.Value is ">>" or "<<")
        {
            var op = cursor.Current.Value;
            cursor.Advance();
            expression = new OperatorExpression(expression, Term(ref cursor, types), op);
        }
        return expression;
    }

    private static Expression Term(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Factor(ref cursor, types);

        while (cursor.Current.Type is TokenType.Operator && cursor.Current.Value is "+" or "-")
        {
            var op = cursor.Current.Value;
            cursor.Advance();
            expression = new OperatorExpression(expression, Factor(ref cursor, types), op);
        }

        return expression;
    }

    private static Expression Factor(ref TokenCursor cursor, TypeLookupTable types)
    {
        var expression = Unary(ref cursor, types);
        //// If it's an identifier and
        //if (expression is IdentifierExpression identifierExpression && cursor.Current.Value is "*")
        //{
        //    var type = types.Find(identifierExpression.Name);
        //    if (type != null)
        //    {

        //    }
        //}

        while (cursor.Current.Type is TokenType.Operator && cursor.Current.Value is "*" or "/" or "%")
        {
            var op = cursor.Current.Value;
            cursor.Advance();
            expression = new OperatorExpression(expression, Unary(ref cursor, types), op);
        }

        return expression;
    }

    private static Expression Unary(ref TokenCursor cursor, TypeLookupTable types)
    {
        if (cursor.Current.Type is TokenType.Operator && cursor.Current.Value is "!" or "-")
        {
            var @operator = cursor.Current.Value;
            cursor.Advance();
            return new UnaryExpression(Unary(ref cursor, types), @operator);
        }
        return Primary(ref cursor, types);
    }

    private static Expression DeReference(ref TokenCursor cursor, TypeLookupTable types)
    {
        //Expression expression = null;
        if (cursor.Is(TokenType.Operator))
        {
            if (cursor.Current.Value == "&")
            {
                cursor.Advance();
                return new AddressOfExpression(Expression(ref cursor, types));
            }
            if (cursor.Current.Value == "*")
            {
                cursor.Advance();
                return new DereferenceExpression(Expression(ref cursor, types));
            }
            // address of
        }

        return Primary(ref cursor, types);

    }

    private static Expression Primary(ref TokenCursor cursor, TypeLookupTable types)
    {
        ref readonly var token = ref cursor.Current;
        if (token.Type is TokenType.Boolean or TokenType.Number or TokenType.Character or TokenType.String)
        {
            // TODO: save the type in some form.
            var literalExpression = new LiteralExpression(token.Value);
            cursor.Advance();
            return literalExpression;

        }
        if (token.Type is TokenType.Identifier)
        {
            var identifierExpression = new IdentifierExpression(token.Value);
            cursor.Advance();
            return identifierExpression;
        }
        if (token.Type == TokenType.PrimitiveType)
        {
            var type = cursor.ReadPrimitiveType();
            cursor.Advance();
            return new IdentifierExpression(type); //TODO: special type here?
        }
        if (token.Type == TokenType.Null)
        {
            cursor.Advance();
            return new NullExpression();
        }
        if (token.Type == TokenType.LeftParenthesis)
        {
            cursor.Advance();
            var groupingExpression = new GroupingExpression(Expression(ref cursor, types));
            if (cursor.Not(TokenType.RightParenthesis))
            {
                throw new Exception($"Failed to find {TokenType.RightParenthesis}");
            }
            cursor.Advance();
            return groupingExpression;
        }
        if (token.Type is TokenType.Operator && token.Value is "&")
        {
            cursor.Advance();
            return new AddressOfExpression(Expression(ref cursor, types));
        }
        if (token.Type is TokenType.Operator && token.Value is "*")
        {
            cursor.Advance();
            return new DereferenceExpression(Expression(ref cursor, types));
        }

        throw new Exception($"Unknown type {token.Type}");
    }

    private static string TranslatePrimitiveType(ref TokenCursor cursor, TypeLookupTable types)
    {
        // TODO: replace string builder with  stackalloc char[512];
        var builder = new StringBuilder();
        while (true)
        {
            builder.Append(cursor.Current.Value);
            if (cursor.Peek().Type == TokenType.PrimitiveType)
            {
                cursor.Advance();
                builder.Append(' ');
            }
            else
            {
                break;
            }
        }
        return builder.ToString();
    }
}