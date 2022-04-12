using System.Diagnostics;

namespace CodeGen.Parser.Expressions;

[DebuggerDisplay("{DebugString()}")]
internal abstract class Expression
{
    public virtual string DebugString() => string.Empty;
}