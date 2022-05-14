using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CodeWriter;

internal class FileOutput
{
    private readonly string _basePath;
    public FileOutput(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task WriteEnum(string fileName, EnumCode enumCode)
    {
        var path = Path.Combine(_basePath, fileName);
        await using var fileStream = File.OpenWrite(path);
        await using var stream = new StreamWriter(fileStream);
        fileStream.Seek(0, SeekOrigin.End);

        var builder = new StringBuilder();
        builder.AppendLine($"internal enum {enumCode.Name}");
        builder.AppendLine("{");
        foreach (var (name, value) in enumCode.Members)
        {
            builder.Append($"\t{name}");
            if (value != null)
            {
                builder.Append($" = {value}");
            }
            builder.AppendLine(",");
        }
        builder.AppendLine("}");
        await stream.WriteAsync(builder.ToString());
    }

    public async Task WriteStruct(string fileName, StructCode structCode)
    {
        var path = Path.Combine(_basePath, fileName);
        await using var fileStream = File.OpenWrite(path);
        await using var stream = new StreamWriter(fileStream);
        fileStream.Seek(0, SeekOrigin.End);

        var builder = new StringBuilder();
        builder.AppendLine($"internal unsafe struct {structCode.Name}");
        builder.AppendLine("{");
        foreach (var (name, value) in structCode.Members)
        {
            builder.AppendLine($"\tpublic {value} {name};");
        }
        builder.AppendLine("}");
        await stream.WriteAsync(builder.ToString());
    }

    public async Task WriteExternFunctions(string fileName, CallingConvention callingConvention, string dllName, string className, IEnumerable<FunctionCode> functions)
    {
        var builder = new StringBuilder();
        var path = Path.Combine(_basePath, fileName);
        await using var fileStream = File.OpenWrite(path);
        await using var stream = new StreamWriter(fileStream);
        fileStream.Seek(0, SeekOrigin.End);

        builder.AppendLine("using System.Runtime.InteropServices;");
        builder.AppendLine();
        builder.AppendLine($"internal unsafe partial class {className}");
        builder.AppendLine("{");
        builder.AppendLine($"\tprivate const string DllName = \"{dllName}\";");
        builder.AppendLine();

        foreach (var function in functions)
        {
            builder.AppendLine($"\t[DllImport(DllName, CallingConvention = CallingConvention.{callingConvention})]");
            builder.Append($"\tpublic static extern {function.ReturnType} {function.Name}");
            if (function.Arguments.Length == 0)
            {
                builder.AppendLine("();");
            }
            else
            {

                var arguments = string.Join($",{Environment.NewLine}", function.Arguments.Select(a => $"\t\t{a.Type} {a.Name}"));
                builder.AppendLine("(");
                builder.AppendLine(arguments);
                builder.AppendLine("\t);");
                builder.AppendLine();
            }
        }

        builder.AppendLine("}");

        await stream.WriteAsync(builder.ToString());
    }
}
