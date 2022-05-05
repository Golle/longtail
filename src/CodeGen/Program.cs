using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeGen;
using CodeGen.Logging;
using CodeGen.Syntax;
using CodeGen.Syntax.Binding;
using CodeGen.Syntax.Statements;


using var _ = Logger.Start();

const string LongtailPath = @"O:\tmp\longtail";

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
        foreach (var input in headers.Take(1))
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

            SyntaxTree tree;
            {
                Logger.Info("Parse started");
                var timer = Stopwatch.StartNew();
                tree = SyntaxTree.Parse(input);
                timer.Stop();
                Logger.Info($"End of file reached. ({timer.Elapsed.TotalMilliseconds:0.##} ms)");
            }

            {
                Logger.Info("Binding started");
                var timer = Stopwatch.StartNew();
                new CodeBinder(new TypeLookupTable())
                    .BindNodes(tree.GetChildren());
                timer.Stop();
                Logger.Info($"Binding completed in ({timer.Elapsed.TotalMilliseconds:0.##} ms)");
            }
            

            //foreach (var syntaxNode in result.GetChildren())
            //{
            //    syntaxNode.PrettyPrint(new SyntaxConsoleWriter());
            //    Logger.Raw(Environment.NewLine);
            //}

            //var types = result.GetChildren()
            //    .GroupBy(c => c.GetType())
            //    .Select(c => (c.Key.Name, Count: c.Count()))
            //    .Select(tuple => $"{tuple.Name,-30}{tuple.Count}");


            //foreach (var typedef in result.GetChildren().OfType<TypedefStatement>())
            //{
            //    typedef.PrettyPrint(new SyntaxConsoleWriter());
            //}

                
            //foreach (var type in types)
            //{
            //    Logger.Raw(type);
            //}


            
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