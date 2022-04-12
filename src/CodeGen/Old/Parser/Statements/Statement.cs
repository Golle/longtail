using System.Diagnostics;

namespace CodeGen.Parser.Statements;

[DebuggerDisplay("{DebugString()}")]
internal abstract class Statement
{
    public virtual string DebugString() => string.Empty;
}