using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using CodeGen;
using CodeGen.CodeWriter;
using CodeGen.Lexer;
using CodeGen.Logging;
using CodeGen.Syntax;


using var _ = Logger.Start();
var baseDir = AppDomain.CurrentDomain.BaseDirectory;
var outPath = @"F:\Git\longtail\src\Longtail\Generated";

var fileOutput = new FileOutput(outPath);
await fileOutput.WriteClass(new CSharpFileDefinition("Longtail_AStruct", new[] { "System", "System.IO" }, "Longtail"));


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
        line = File.ReadAllText(@"O:\tmp\longtail\src\longtail.h");
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

    var parser = new Parser(line);

    Logger.Info($"Successfully parsed {parser._tokens.Length} tokens");

    //for (var i = 0; i < parser._tokens.Length; ++i)
    //{
    //    var token = parser._tokens[i];

    //    if (token.Type == TokenType.Struct)
    //    {
    //        for (var x = 0; x < 4; ++x)
    //        {
    //            var t = parser._tokens[i+x];
    //            Console.Write( t + " ");
    //        }
    //        Console.WriteLine();
    //    }
    //}

    //continue;
    foreach (var token in parser._tokens)
    {

        if (token.Type != TokenType.NewLine)
        {
            Console.Write($"{token.Type} ");
        }
        else
        {
            Console.WriteLine();
        }
        
    }
    //var lexer = new Lexer(line);

    //Token token;
    //while ((token = lexer.Lex()).Kind != TokenKind.EndOfFile)
    //{
    //    Console.WriteLine(token);
    //}
    Console.WriteLine("End of file reached.");
}



//var lookupTable = new Dictionary<string, TokenType>()
//{
//    { "LONGTAIL_EXPORT", TokenType.DllExport }
//};

//var typeLookupTable = TypeLookupTable.CreateDefault()
//    .AddTypedef("uint64_t","unsigned long long")
//    .AddTypedef("uint32_t", "unsigned int");



//foreach (var headerFile in headerFiles.Take(1))
//{
//    Logger.Info($"------------- {headerFile} ------------- ");
//    var contents = File.ReadAllText(headerFile);
//    var tokens = new Tokenizer(true, lookupTable)
//            .Tokenize(contents)
//            .ToArray();
//    var node = new LongtailParser(typeLookupTable)
//        .Parse(tokens);

//    Logger.Info($"------------- {headerFile} -------------\n\n");
//}

