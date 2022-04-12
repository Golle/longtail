namespace CodeGen.Parser.Types;

internal class PrimitiveTypeDeclaration : TypeDeclaration
{
    public PrimitiveTypeDeclaration(string name, TypeDeclaration? baseType = null)
        :base(name, baseType)
    {
    }
}