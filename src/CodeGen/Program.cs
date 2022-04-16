using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using CodeGen;
using CodeGen.CodeWriter;
using CodeGen.Lexer;
using CodeGen.Logging;
using CodeGen.Syntax;


using var _ = Logger.Start();

var sampleCode = @"
struct 

";


Console.WriteLine("Welcome to the interpreter!");

while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line == null)
    {
        continue;
    }

    if (line.Length == 0)
    {
        //line = File.ReadAllText(@"O:\tmp\longtail\src\longtail.h");
    }
    else if (line.StartsWith("#longtail"))
    {
        line = new LongtailPreParser(@"O:\tmp\longtail")
            .Run();
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
            line = File.ReadAllText(splits[1]);
        }
        else
        {
            Console.WriteLine($"File {splits[1]} does not exist.");
            continue;
        }
    }

    var result = new Parser(line)
        .Parse();

    foreach (var syntaxNode in result.GetChildren())
    {
        syntaxNode.PrettyPrint();
    }
    
    Console.WriteLine("End of file reached.");
}




// Test output code
//var outPath = @"F:\Git\longtail\src\Longtail\Generated";

//var fileOutput = new FileOutput(outPath);
//await fileOutput.WriteClass(new CSharpFileDefinition("Longtail_AStruct", new[] { "System", "System.IO" }, "Longtail"));