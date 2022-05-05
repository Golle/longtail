using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.Lexer;
using CodeGen.Logging;
using CodeGen.Syntax.Expressions;
using CodeGen.Syntax.Statements;
using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;



internal class BuiltInTypeComparer : IEqualityComparer<TokenType[]>
{
    public bool Equals(TokenType[]? x, TokenType[]? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x.Length != y.Length)
        {
            return false;
        }

        var result = 0;
        for (var i = 0; i < x.Length; ++i)
        {
            unchecked
            {
                result = x[i] - y[i];
            }
        }
        return result == 0;
    }

    public int GetHashCode(TokenType[] obj)
    {
        var result = 0;
        foreach (var tokenType in obj)
        {
            result += (int)tokenType;
        }
        return result;
    }
}
internal class TypeLookupTable
{

    private List<TypeSymbol> _symbols = new();
    
    private Dictionary<TokenType[], TypeSymbol> _builtInSymbols = new (new BuiltInTypeComparer());

    public TypeLookupTable AddSymbolDefintion(TypeSymbol symbol)
    {


        return this;
    }
    
    public TypeSymbol? Find(TokenType[] types)
    {

        


        return null;
    }
}

internal class CodeBinder
{
    private readonly TypeLookupTable _typeLookupTable;
    public CodeBinder(TypeLookupTable typeLookupTable)
    {
        _typeLookupTable = typeLookupTable;
    }

    public void BindNodes(IEnumerable<SyntaxNode> nodes)
    {
        foreach (var syntaxNode in nodes)
        {
            Logger.Info($"Binding {syntaxNode.GetType().Name}");

            switch (syntaxNode)
            {
                case FunctionDeclarationStatement statement:
                    BindFunctionDeclaration(statement);
                    break;
                case StructDeclarationStatement statement:
                    BindStructDeclaration(statement);
                    break;
                case VariableDeclarationStatement statement:
                    BindVariableDeclarationStatement(statement);
                    break;

            }
        }
    }

    private object BindVariableDeclarationStatement(VariableDeclarationStatement statement)
    {
        // NOTE(Jens): restrict what we support to keep it simple

        var type = statement.Type switch
        {
            BuiltInTypeExpression builtInType => LookupBuiltInType(builtInType),
            PointerTypeExpression pointer => BindPointerType(pointer),
            _ => throw new NotSupportedException()
        };

        return new object();

    }

    private TypeSymbol LookupBuiltInType(BuiltInTypeExpression builtInType)
    {
        var type = _typeLookupTable.Find(builtInType.Types);
        if (type == null)
        {
            throw new BindException($"Failed to find symbol definition for {string.Join(' ', builtInType.Types)}");
        }
        return type;
    }

    private TypeSymbol BindPointerType(PointerTypeExpression pointer)
    {
        throw new System.NotImplementedException();
    }

    private void BindStructDeclaration(StructDeclarationStatement statement)
    {
        //throw new System.NotImplementedException();
    }

    private void BindFunctionDeclaration(FunctionDeclarationStatement statement)
    {
        //Logger.Info("Bind function lol");
    }
}

internal class BindException : Exception
{
    public BindException(string message) : base(message)
    {
    }
}