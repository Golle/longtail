using System.Diagnostics;

namespace CodeGen.Parser.Types;

[DebuggerDisplay("{ToString()}")]
internal abstract class TypeDeclaration
{
    public string Name { get; }
    public bool Pointer { get; }
    public TypeDeclaration? BaseType { get; protected set; }

    protected TypeDeclaration(string name, TypeDeclaration? baseType = null, bool pointer = false)
    {
        Name = name;
        BaseType = baseType;
        Pointer = pointer;
    }
    public string GetBaseType() => BaseType?.GetBaseType() ?? Name;

    public override string ToString() => Pointer ? $"*{Name}" : Name;
}