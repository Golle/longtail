namespace CodeGen.Parser.Types;

internal class TypedefDeclaration : TypeDeclaration
{
    public TypedefDeclaration(string name, TypeDeclaration baseType, bool pointer) 
        : base(name, baseType, pointer)
    {
    }
}