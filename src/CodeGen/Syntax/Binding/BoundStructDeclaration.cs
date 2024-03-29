﻿using CodeGen.Syntax.Statements;
using CodeGen.Syntax.Symbols;

namespace CodeGen.Syntax.Binding;

internal class BoundStructDeclaration : BoundStatement
{
    public StructTypeSymbol Type { get; }
    public bool ForwardDeclaration { get; }

    public BoundStructDeclaration(Statement statement, StructTypeSymbol type, bool forwardDeclaration)
        : base(statement)
    {
        Type = type;
        ForwardDeclaration = forwardDeclaration;
    }
}