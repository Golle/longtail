using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CodeGen;
using CodeGen.CodeWriter;
using CodeGen.Lexer;
using CodeGen.Logging;
using CodeGen.Syntax;
using CodeGen.Syntax.Binding;

using var _ = Logger.Start();

const string SampleOutputBasePath = @"o:\tmp";
const string LongtailOutputBasePath = @"F:\Git\longtail\src\Longtail\Generated\";
const string LongtailOutputFileName = "Longtail.cs";
const string LongtailOutputLibraryFileName = "LongtailLibrary.cs";
const string LongtailPath = @"O:\tmp\longtail";
const string SamplePath = @"O:\tmp\";

Console.WriteLine("Welcome to the interpreter!");
while (true)
{
    await Task.Delay(50);
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line == null)
    {
        continue;
    }

    if (line.Length == 0)
    {
        var result = Parse(File.ReadAllText(Path.Combine(SamplePath, "sample.h")));
        await ExportCSharpCode(result, SampleOutputBasePath, "sample.cs", true);
        await ExportFunctions(result?.Where(c => c is FunctionCode).Cast<FunctionCode>() ?? Array.Empty<FunctionCode>(), SampleOutputBasePath, "sample_functions.cs", "Sample");
    }
    else if (line.StartsWith("#longtail"))
    {
        var timer = Stopwatch.StartNew();
        var headers = new LongtailFileReader(LongtailPath)
            .EnumerateAllHeaders(("LONGTAIL_EXPORT", "__declspec(dllexport)"));
        var resetFile = true;

        List<FunctionCode> functions = new();
        foreach (var input in headers)
        {
            var result = Parse(input);
            await ExportCSharpCode(result, LongtailOutputBasePath, LongtailOutputFileName, resetFile);
            resetFile = false;

            functions.AddRange(result?.Where(c => c is FunctionCode).Cast<FunctionCode>() ?? Array.Empty<FunctionCode>());
        }
        await ExportFunctions(functions, LongtailOutputBasePath, LongtailOutputLibraryFileName, "LongtailLibrary");
        var typedefStructs = new[]
        {
            "Longtail_CancelAPI_CancelToken",
            "Longtail_StorageAPI_OpenFile",
            "Longtail_StorageAPI_Iterator",
            "Longtail_StorageAPI_LockFile",
            "Longtail_StorageAPI_FileMap",
            "Longtail_ChunkerAPI_Chunker",
            "Longtail_HashAPI_Context",
            "Longtail_LookupTable",
            "Longtail_Paths"
        };
        foreach (var typedefStruct in typedefStructs)
        {
            var code = new CSharpCode[] { new StructCode(typedefStruct, Array.Empty<StructMember>()) };
            await ExportCSharpCode(code, LongtailOutputBasePath, LongtailOutputFileName, false);
        }

        timer.Stop();
        Logger.Info($"Generated longtail bindings in {timer.Elapsed.TotalMilliseconds} ms");
    }
    else if (line.StartsWith("#file"))
    {
        var splits = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (splits.Length == 1)
        {
            Console.WriteLine("Failed to get the path of the file");
            continue;
        }

        if (File.Exists(splits[1]))
        {
            var result = Parse(File.ReadAllText(splits[1]));
            // TODO: add export here as well
        }
        else
        {
            Console.WriteLine($"File {splits[1]} does not exist.");
        }
    }
    else
    {
        var result = Parse(line);
    }

    static CSharpCode[]? Parse(string input)
    {
        var tree = Run("Parse Syntax Tree", () => SyntaxTree.Parse(input));
        if (tree == null)
        {
            return null;
        }

        var nodes = Run("Create AST", () => new CodeBinder(new SymbolLookupTable()
                .AddTypedef("uint64_t", TokenType.Unsigned, TokenType.Long, TokenType.Long)
                .AddTypedef("uint32_t", TokenType.Unsigned, TokenType.Int)
                .AddTypedef("uint16_t", TokenType.Unsigned, TokenType.Short)
                .AddTypedef("uint8_t", TokenType.Char)
                .AddTypedef("size_t", TokenType.Unsigned, TokenType.Long, TokenType.Long))
            .BindNodes(tree.GetChildren()));

        if (nodes == null)
        {
            return null;
        }

        return Run("Bind code", () => new CSharpCodeGen()
            .GenerateCode(nodes));
    }
}


static T? Run<T>(string name, Func<T> func)
{
    Logger.Trace($"{name} started");
    var timer = Stopwatch.StartNew();
    try
    {
        return func();
    }
    catch (Exception e)
    {
        Logger.Error($"{name} failed with {e.GetType().Name} and message: {e.Message}");
        return default;
    }
    finally
    {
        Logger.Trace($"{name} finished in {timer.Elapsed.TotalMilliseconds:0.##} ms");
    }

}


async Task ExportCSharpCode(CSharpCode[]? code, string basePath, string fileName, bool resetFile)
{
    if (code == null)
    {
        return;
    }

    if (resetFile)
    {
        var fullPath = Path.Combine(basePath, fileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
    var output = new FileOutput(basePath);
    foreach (var cSharpCode in code)
    {
        if (cSharpCode is EnumCode enumCode)
        {
            await output.WriteEnum(fileName, enumCode);
        }
        else if (cSharpCode is StructCode structCode)
        {
            await output.WriteStruct(fileName, structCode);
        }
    }

    var functions = code.Where(c => c is FunctionCode).Cast<FunctionCode>();
}

async Task ExportFunctions(IEnumerable<FunctionCode> functions, string basePath, string fileName, string className)
{
    var fullPath = Path.Combine(basePath, fileName);
    if (File.Exists(fullPath))
    {
        File.Delete(fullPath);
    }
    var output = new FileOutput(basePath);
    await output.WriteExternFunctions(fileName, CallingConvention.Cdecl, "longtail", className, functions);
}