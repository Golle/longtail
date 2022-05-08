using System;
using System.Collections.Generic;
using CodeGen.Lexer;
using CodeGen.Logging;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;

internal class CodeBinder
{
    private readonly SymbolLookupTable _symbolLookupTable;
    public CodeBinder(SymbolLookupTable symbolLookupTable)
    {
        _symbolLookupTable = symbolLookupTable;
    }

    public void BindNodes(IEnumerable<SyntaxNode> nodes)
    {
        List<BoundSyntaxNode> boundNodes = new();
        foreach (var syntaxNode in nodes)
        {
            Logger.Info($"Binding {syntaxNode.GetType().Name}");

            switch (syntaxNode)
            {
                case FunctionDeclarationStatement statement:
                    boundNodes.Add(BindFunctionDeclaration(statement));
                    break;
                case StructDeclarationStatement statement:
                    boundNodes.Add(BindStructDeclaration(statement));
                    break;
                case VariableDeclarationStatement statement:
                    boundNodes.Add(BindVariableDeclarationStatement(statement));
                    break;
                case TypedefStatement statement:

                    if (statement.Definition is ExpressionStatement expression)
                    {
                        var type = LookupSymbol(expression.Expression);
                        _symbolLookupTable.AddTypedef(statement.Name, type);
                    }
                    
                    break;
            }
        }
    }

    private BoundVariableDeclaration BindVariableDeclarationStatement(VariableDeclarationStatement statement)
    {
        // NOTE(Jens): restrict what we support to keep it simple
        var type = LookupSymbol(statement.Type);

        return new BoundVariableDeclaration(statement, type, BindExpression(statement.Variable));
    }

    private BoundExpression BindExpression(Expression expression)
    {
        var a = expression switch
        {
            BinaryExpression binary => new BoundBinaryExpression(binary, BindExpression(binary.Left), BindExpression(binary.Right), binary.Operator),
            AssigmentExpression assignment => new BoundAssignmentExpression(assignment, BindExpression(assignment.Left), BindExpression(assignment.Right), assignment.Operator),
            LiteralExpression literal => BindLiteral(literal),
            IdentifierExpression identifier => new BoundIdentifierExpression(identifier, identifier.Value),
            _ => throw new NotImplementedException($"Binding for {expression.GetType()} has not been implemented.")
        };

        return a;
    }

    private BoundExpression BindLiteral(LiteralExpression literal)
    {
        var typeSymbol = _symbolLookupTable.Find(literal.Type) ?? throw new BinderException($"No type symbol found for token {literal.Type}.");
        return new BoundLiteralExpression(literal, typeSymbol, literal.Value);
    }

    private TypeSymbol BindPointerType(PointerTypeExpression pointerType)
    {
        var type = pointerType.Expression switch
        {
            PointerTypeExpression pointer => BindPointerType(pointer),
            BuiltInTypeExpression builtInType => LookupBuiltInType(builtInType),
            _ => throw new NotImplementedException($"Binding for {pointerType.Expression.GetType()} has not been implemented.")
        };
        return type;
    }

    private BoundStructDeclaration BindStructDeclaration(StructDeclarationStatement statement)
    {
        var type = new StructTypeSymbol(statement.Name);
        _symbolLookupTable.RegisterType(type, !statement.ForwardDeclaration);

        // TODO: Add members
        return new BoundStructDeclaration(statement, type);
    }

    private BoundFunctionDeclaration BindFunctionDeclaration(FunctionDeclarationStatement statement)
    {
        var returnType = LookupSymbol(statement.ReturnType);

        return new BoundFunctionDeclaration(statement);
    }

    private Symbol LookupSymbol(Expression expression) =>
        expression switch
        {
            StructExpression structExpression => LookupStructExpressionSymbol(structExpression),
            PointerTypeExpression pointer => new PointerTypeSymbol(LookupSymbol(pointer.Expression)),
            ReferenceTypeExpression reference => new ReferenceTypeSymbol(LookupSymbol(reference.Expression)),
            BuiltInTypeExpression builtIn => LookupBuiltInType(builtIn),
            IdentifierExpression identifer => LookupType(identifer),
            ConstExpression constExpression => new ConstSympol(LookupSymbol(constExpression.Expression)),
            _ => throw new NotImplementedException($"Binding for symbol {expression.GetType()} has not been implemented.")
        };

    private Symbol LookupStructExpressionSymbol(StructExpression structExpression)
    {
        if (structExpression.Expression is PointerTypeExpression { Expression: IdentifierExpression identifier } pointer)
        {
            var symbol = _symbolLookupTable.Find(identifier.Value);
            if (symbol == null)
            {
                // NOTE(Jens): Special case where the typedef struct has not been defined anywhere else. For example typedef struct Struct* StructH;
                var structSymbol = new StructTypeSymbol(identifier.Value);
                _symbolLookupTable.RegisterType(structSymbol);
                return structSymbol;
            }
            return symbol;
        }
        throw new BinderException($"Unexpected struct expression type: {structExpression.Expression.GetType()}");
    }

    private PrimitiveTypeSymbol LookupBuiltInType(BuiltInTypeExpression builtInType) =>
        _symbolLookupTable.Find(builtInType.Types) ?? throw new BinderException($"Failed to find symbol definition for {string.Join(' ', builtInType.Types)}");

    private Symbol LookupType(IdentifierExpression identifier) =>
        _symbolLookupTable.Find(identifier.Value) ?? throw new BinderException($"Failed to find symbol definition for {identifier.Value}");
}