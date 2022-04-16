using System;
using System.IO;
using System.Threading.Tasks;
using CodeGen.Logging;

namespace CodeGen.CodeWriter;

internal class FileOutput
{
    private readonly string _basePath;
    public FileOutput(string basePath)
    {
        _basePath = basePath;
    }

    public async Task WriteClass(CSharpFileDefinition fileDefinition)
    {
        var path = Path.Combine(_basePath, fileDefinition.Namespace.Replace('.', Path.DirectorySeparatorChar), $"{fileDefinition.Name}.cs");
        var directory = Path.GetDirectoryName(path) ?? throw new InvalidOperationException($"Failed to get the directory name of {path}");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        Logger.Info($"Wring class {fileDefinition.Name} to {path}");
        await using var stream = new StreamWriter(File.OpenWrite(path));
        foreach (var usings in fileDefinition.Usings)
        {
            await stream.WriteLineAsync($"using {usings};");
        }
        await stream.WriteLineAsync("//This is just a comment.");
        await stream.WriteLineAsync($"namespace {fileDefinition.Namespace};");

        await stream.WriteLineAsync($"internal struct {fileDefinition.Name} {{ }}");
        await stream.FlushAsync();
    }
}


internal class CSharpFileDefinition
{
    public string Name { get; }
    public string[] Usings { get; }
    public string Namespace { get; }
    public CSharpFileDefinition(string name, string[] usings, string ns)
    {
        Name = name;
        Usings = usings;
        Namespace = ns;
    }
}