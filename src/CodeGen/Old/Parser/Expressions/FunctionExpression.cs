using System.Collections.Generic;
using System.Linq;

namespace CodeGen.Parser.Expressions;

internal class FunctionExpression : Expression
{
    private readonly Expression _callee;
    private readonly List<Expression> _arguments;
    public FunctionExpression(Expression callee, List<Expression> arguments)
    {
        _callee = callee;
        _arguments = arguments;
    }
    public override string DebugString()
    {
        return $"{_callee.DebugString()}({string.Join(", ", _arguments.Select(a => a.DebugString()))})";
    }
}