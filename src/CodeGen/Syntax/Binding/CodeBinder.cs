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

                    break;
            }
        }
        return boundNodes.ToArray();
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
        var type = new StructTypeSymbol(statement.Name);
        _symbolLookupTable.RegisterType(type, !statement.ForwardDeclaration);

        // TODO: Add members
        return new BoundStructDeclaration(statement, type);
    }

    private BoundFunctionDeclaration BindFunctionDeclaration(FunctionDeclarationStatement statement)
    {
        var functionArguments = statement.Arguments.Select(a =>
            {
                var identifier = a.Variable as IdentifierExpression ?? throw new NotSupportedException($"{a.Variable.GetType()} is not supported as the variable declaration.");
                var symbol = LookupSymbol(a.Type);
                return new FunctionArgument(identifier.Value, symbol);
            })
            .ToArray();

        var returnType = LookupSymbol(statement.ReturnType);

        var symbol = new FunctionSymbol(statement.Name, returnType, functionArguments);
        _symbolLookupTable.RegisterType(symbol);
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
            ConstExpression constExpression => new ConstSympol(LookupSymbol(constExpression.Expression)),
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
            var structSymbol = new StructTypeSymbol(identifier);
            _symbolLookupTable.RegisterType(structSymbol);
            return structSymbol;
        }
    }

    private PrimitiveTypeSymbol LookupBuiltInType(BuiltInTypeExpression builtInType) =>
        _symbolLookupTable.Find(builtInType.Types) ?? throw new BinderException($"Failed to find symbol definition for {string.Join(' ', builtInType.Types)}");

    private Symbol LookupType(IdentifierExpression identifier) =>
        _symbolLookupTable.Find(identifier.Value) ?? throw new BinderException($"Failed to find symbol definition for {identifier.Value}");
}

internal class FunctionArgument
{
    public string Name { get; }
    public Symbol Symbol { get; }

    public FunctionArgument(string name, Symbol symbol)
    {
        Name = name;
        Symbol = symbol;
    }
}

internal class FunctionSymbol : TypeSymbol
{
    public Symbol ReturnType { get; }
    public FunctionArgument[] Arguments { get; }
    public FunctionSymbol(string name, Symbol returnType, FunctionArgument[] arguments) 
        : base(name)
    {
        ReturnType = returnType;
        Arguments = arguments;
    }
}