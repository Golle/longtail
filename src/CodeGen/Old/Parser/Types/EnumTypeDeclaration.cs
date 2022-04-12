using System.Text;

namespace CodeGen.Parser.Types;

internal class EnumTypeDeclaration : TypeDeclaration
{
    private readonly EnumMember[] _members;
    public EnumTypeDeclaration(string name, EnumMember[] members, TypeDeclaration? baseType) 
        : base(name, baseType)
    {
        _members = members;
    }

    public override string ToString()
    {
        StringBuilder builder = new ();
        builder.Append($"enum {Name}");
        if (BaseType != null)
        {
            builder.Append($" : {BaseType.Name}");
        }

        builder.Append('{');
        for (var i = 0; i < _members.Length; ++i)
        {
            ref readonly var member = ref _members[i];
            builder.Append(member.Name);
            if (member.Expression != null)
            {
                builder.Append($" = {member.Expression}");
            }

            if (i < _members.Length - 1)
            {
                builder.Append(',');
            }
        }

        builder.Append('}');
        return builder.ToString();
    }
}