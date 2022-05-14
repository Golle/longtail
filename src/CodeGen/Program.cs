using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeGen;
using CodeGen.CodeWriter;
using CodeGen.Lexer;
using CodeGen.Logging;
using CodeGen.Syntax;
using CodeGen.Syntax.Binding;


using var _ = Logger.Start();

const string LongtailPath = @"O:\tmp\longtail";

Console.WriteLine("Welcome to the interpreter!");
var resetFile = true;
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
        await Parse(File.ReadAllText(@"O:\tmp\sample.h"));
    }
    else if (line.StartsWith("#longtail"))
    {
        var headers = new LongtailFileReader(LongtailPath)
            .EnumerateAllHeaders(("LONGTAIL_EXPORT", "__declspec(dllexport)"));
        foreach (var input in headers.Take(1))
        {
            await Parse(input);
        }
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
            await Parse(File.ReadAllText(splits[1]));
        }
        else
        {
            Console.WriteLine($"File {splits[1]} does not exist.");
        }
    }
    else
    {
        await Parse(line);
    }

    async Task<SyntaxTree?> Parse(string input)
    {
        try
        {

            SyntaxTree tree;
            {
                Logger.Info("Parse started");
                var timer = Stopwatch.StartNew();
                tree = SyntaxTree.Parse(input);
                timer.Stop();
                Logger.Info($"End of file reached. ({timer.Elapsed.TotalMilliseconds:0.##} ms)");

                //foreach (var syntaxNode in tree.GetChildren())
                //{
                //    syntaxNode.PrettyPrint(new SyntaxConsoleWriter());
                //    Logger.Raw(Environment.NewLine);
                //}
            }

            BoundSyntaxNode[] nodes;
            {
                Logger.Info("Binding started");
                var timer = Stopwatch.StartNew();

                var lookupTable = new SymbolLookupTable()
                        .AddTypedef("uint64_t", TokenType.Unsigned, TokenType.Long, TokenType.Long)
                        .AddTypedef("uint32_t", TokenType.Unsigned, TokenType.Int)
                        .AddTypedef("uint16_t", TokenType.Unsigned, TokenType.Short)
                        .AddTypedef("uint8_t", TokenType.Char)
                        .AddTypedef("size_t", TokenType.Unsigned, TokenType.Long, TokenType.Long) // This is for x64
                    ;
                nodes = new CodeBinder(lookupTable)
                    .BindNodes(tree.GetChildren());


                //foreach (var boundSyntaxNode in nodes)
                //{
                //    Logger.Info(boundSyntaxNode.ToString() ?? string.Empty);
                //}


                timer.Stop();
                Logger.Info($"Binding completed in ({timer.Elapsed.TotalMilliseconds:0.##} ms)");
            }

            CSharpCode[] code;
            {
                Logger.Info("Code gen started");
                var timer = Stopwatch.StartNew();
                code = new CSharpCodeGen()
                    .GenerateCode(nodes);

                timer.Stop();
                Logger.Info($"Binding completed in ({timer.Elapsed.TotalMilliseconds:0.##} ms)");

            }


            {

                var output = new FileOutput(@"O:\tmp\");
                // these structs are not defined anywhere.
                var typedefStructs = new[]
                {
                    "Longtail_CancelAPI_CancelToken",
                    "Longtail_StorageAPI_OpenFile",
                    "Longtail_StorageAPI_Iterator",
                    "Longtail_StorageAPI_LockFile",
                    "Longtail_StorageAPI_FileMap",
                    "Longtail_ChunkerAPI_Chunker",
                    "Longtail_HashAPI_Context"
                };
                foreach (var typedefStruct in typedefStructs)
                {
                    await output.WriteStruct("sample.cs", new StructCode(typedefStruct, Array.Empty<StructMember>()), resetFile: resetFile);
                    resetFile = false;
                }
                
                foreach (var cSharpCode in code)
                {
                    if (cSharpCode is EnumCode enumCode)
                    {
                        await output.WriteEnum("sample.cs", enumCode, resetFile);
                    }
                    else if (cSharpCode is StructCode structCode)
                    {
                        await output.WriteStruct("sample.cs", structCode, resetFile);
                    }
                }
                
            }


            return tree;
        }
        catch (ParserException e)
        {
            Logger.Error($"Parser failed with message {e.Message}");
        }
        return null;
    }
}





// Test output code
//var outPath = @"F:\Git\longtail\src\Longtail\Generated";

//var fileOutput = new FileOutput(outPath);
//await fileOutput.WriteClass(new CSharpFileDefinition("Longtail_AStruct", new[] { "System", "System.IO" }, "Longtail"));