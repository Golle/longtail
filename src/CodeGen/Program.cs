using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeGen;
using CodeGen.Logging;
using CodeGen.Syntax;


using var _ = Logger.Start();

const string LongtailPath = @"O:\tmp\longtail";

//var longtailLibPath = Path.Combine(LongtailPath, "lib");
//var longtailHeader = Path.Combine(LongtailPath, "src", "longtail.h");

//var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
//    .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly))
//    .ToArray();


//foreach (var headerFile in headerFiles)
//{
//    Logger.Trace($"Reading contents: {headerFile}");
//    var t = File.ReadAllText(headerFile);
//    new Parser(t).Parse();
//}

//return;

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
        Parse(File.ReadAllText(@"O:\tmp\sample.h"));
    }
    else if (line.StartsWith("#longtail"))
    {
        var headers = new LongtailFileReader(LongtailPath)
            .EnumerateAllHeaders(("LONGTAIL_EXPORT", "__declspec(dllexport)"));
        foreach (var input in headers)
        {
            Parse(input);
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
            Parse(File.ReadAllText(splits[1]));
        }
        else
        {
            Console.WriteLine($"File {splits[1]} does not exist.");
        }
    }
    else
    {
        Parse(line);
    }

    static SyntaxTree? Parse(string input)
    {
        try
        {
            var timer = Stopwatch.StartNew();
            var result = new Parser(input)
                .Parse();
            timer.Stop();
            Console.WriteLine("Syntax tree.");
            foreach (var syntaxNode in result.GetChildren())
            {
                syntaxNode.PrettyPrint(new SyntaxConsoleWriter());
                Logger.Raw(Environment.NewLine);
            }

            Logger.Raw($"End of file reached. ({timer.Elapsed.TotalMilliseconds:0.##} ms)");
            return result;
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