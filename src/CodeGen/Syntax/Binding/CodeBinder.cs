using System;
using System.Collections.Generic;
using System.Linq;
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

    public BoundSyntaxNode[] BindNodes(IEnumerable<SyntaxNode> nodes)
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
                    else
                    {
                        Logger.Warning($"No handling implemented for typedef {statement.Definition.GetType().Name}");
                    }
                    break;
                case EnumDeclarationStatement statement:
                    boundNodes.Add(BindEnumStatement(statement));
                    break;
                default:
                    Logger.Warning($"No handling implemented for {syntaxNode.GetType().Name}");
                    break;
            }
        }
        
        return boundNodes.ToArray();
    }

    private BoundSyntaxNode BindEnumStatement(EnumDeclarationStatement statement)
    {
        var enumType = new EnumTypeSymbol(statement.Name);
        if (!string.IsNullOrWhiteSpace(statement.Name))
        {
            // only added named enums (does not exist in longtail and its a c++ feature)
            _symbolLookupTable.AddType(enumType);
        }

        // TODO(Jens): these values are not referenced anywhere in the longtail header file so we don't have to register them as types right now
        var boundMembers = statement
            .Members
            .Select(m => m switch
            {
                AssigmentExpression assignment => new BoundAssignmentExpression(assignment, BindExpression(assignment.Left), BindExpression(assignment.Right), assignment.Operator),
                IdentifierExpression identifier => (BoundExpression)new BoundIdentifierExpression(identifier, identifier.Value),
                _ => throw new NotImplementedException($"Binding for enum member of type {m.GetType()} has not been implemented.")
            })
            .ToArray();

        return new BoundEnumDeclaration(statement, enumType, boundMembers);
    }

    private BoundVariableDeclaration BindVariableDeclarationStatement(VariableDeclarationStatement statement)
    {
        // NOTE(Jens): restrict what we support to keep it simple
        var type = LookupSymbol(statement.Type);

        return new BoundVariableDeclaration(statement, type, BindExpression(statement.Variable));
    }

    private BoundExpression BindExpression(Expression expression)
    {
        var boundExpression = expression switch
        {
            BinaryExpression binary => new BoundBinaryExpression(binary, BindExpression(binary.Left), BindExpression(binary.Right), binary.Operator),
            AssigmentExpression assignment => new BoundAssignmentExpression(assignment, BindExpression(assignment.Left), BindExpression(assignment.Right), assignment.Operator),
            LiteralExpression literal => BindLiteral(literal),
            IdentifierExpression identifier => new BoundIdentifierExpression(identifier, identifier.Value),
            _ => throw new NotImplementedException($"Binding for {expression.GetType()} has not been implemented.")
        };
        return boundExpression;
    }

    private BoundExpression BindLiteral(LiteralExpression literal)
    {
        var typeSymbol = _symbolLookupTable.Find(literal.Type) ?? throw new BinderException($"No type symbol found for token {literal.Type}.");
        return new BoundLiteralExpression(literal, typeSymbol, literal.Value);
    }

    private BoundStructDeclaration BindStructDeclaration(StructDeclarationStatement statement)
    {
        var members = statement
            .Members
            .Select(m =>
            {
                var type = LookupSymbol(m.Type);
                if (m.Variable is ArrayExpression array)
                {
                    var initializer = BindExpression(array.Expression!);
                    
                    // NOTE(Jens): assume identifier expression here.
                    return new ArrayStructMember(((IdentifierExpression)array.Left).Value, type, initializer);
                }

                if (m.Variable is IdentifierExpression identifier)
                {
                    return new StructMember(identifier.Value, type);
                }

                throw new BinderException($"{m.Variable.GetType().Name} is not supported");
            })
            .ToArray();

        var type = new StructTypeSymbol(statement.Name, members);
        _symbolLookupTable.AddType(type, !statement.ForwardDeclaration);

        return new BoundStructDeclaration(statement, type, statement.ForwardDeclaration);
    }


    private BoundFunctionDeclaration BindFunctionDeclaration(FunctionDeclarationStatement statement)
    {
        var functionArguments = statement.Arguments.Select(a =>
            {
                var symbol = LookupSymbol(a.Type);
                if (a.Variable is ArrayExpression array)
                {
                    if (array.Expression != null)
                    {
                        throw new NotSupportedException("Fixed size arrays are not supported in function arguments");
                    }

                    //NOTE(Jens): Treat type[] as pointer
                    var identifier = array.Left as IdentifierExpression ?? throw new BinderException($"Expected {nameof(IdentifierExpression)} but found {array.Left.GetType().Name}");
                    return new FunctionArgument(identifier.Value, new PointerTypeSymbol(symbol));
                }
                else if (a.Variable is IdentifierExpression identifier)
                {
                    return new FunctionArgument(identifier.Value, symbol);
                }
                throw new NotSupportedException($"{a.Variable.GetType()} is not supported as the variable declaration.");
            })
            .ToArray();

        var returnType = LookupSymbol(statement.ReturnType);

        var symbol = statement.IsFunctionPointer
            ? new FunctionPointerSymbol(statement.Name, returnType, functionArguments)
            : new FunctionSymbol(statement.Name, returnType, functionArguments, statement.Modifiers.Contains(TokenType.DllExport));

        _symbolLookupTable.AddType(symbol);
        return new BoundFunctionDeclaration(statement, symbol);
    }

    private Symbol LookupSymbol(Expression expression) =>
        expression switch
        {
            StructExpression structExpression => LookupStructExpressionSymbol(structExpression.Expression),
            PointerTypeExpression pointer => new PointerTypeSymbol(LookupSymbol(pointer.Expression)),
            ReferenceTypeExpression reference => new ReferenceTypeSymbol(LookupSymbol(reference.Expression)),
            BuiltInTypeExpression builtIn => LookupBuiltInType(builtIn),
            IdentifierExpression identifer => LookupType(identifer),
            ConstExpression constExpression => new ConstSymbol(LookupSymbol(constExpression.Expression)),
            _ => throw new NotImplementedException($"Binding for symbol {expression.GetType()} has not been implemented.")
        };

    private Symbol LookupStructExpressionSymbol(Expression structExpression)
    {
        return structExpression switch
        {
            PointerTypeExpression pointer => new PointerTypeSymbol(LookupStructExpressionSymbol(pointer.Expression)),
            IdentifierExpression identifier1 => _symbolLookupTable.Find(identifier1.Value) ?? CreateStrutType(identifier1.Value),
            _ => throw new BinderException($"Unexpected struct expression type: {structExpression.GetType()}")
        };
        Symbol CreateStrutType(string identifier)
        {
            // NOTE(Jens): Special case where the typedef struct has not been defined anywhere else. For example typedef struct Struct* StructH;
            var structSymbol = new StructTypeSymbol(identifier, Array.Empty<StructMember>());
            _symbolLookupTable.AddType(structSymbol);
            return structSymbol;
        }
    }

    private PrimitiveTypeSymbol LookupBuiltInType(BuiltInTypeExpression builtInType) =>
        _symbolLookupTable.Find(builtInType.Types) ?? throw new BinderException($"Failed to find symbol definition for {string.Join(' ', builtInType.Types)}");

    private Symbol LookupType(IdentifierExpression identifier) =>
        _symbolLookupTable.Find(identifier.Value) ?? throw new BinderException($"Failed to find symbol definition for {identifier.Value}");
}