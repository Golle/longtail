using System;
using CodeGen;
using CodeGen.Logging;


using var _ = Logger.Start();



Console.WriteLine("Welcome to the interpreter!");
while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line == null)
    {
        continue;
    }

    var lexer = new Lexer(line);

    Token token;
    while ((token = lexer.Lex()).Kind != TokenKind.EndOfFile)
    {
        Console.WriteLine(token);
    }
    Console.WriteLine("End of file reached.");
}




//const string longtailBasePath = @"O:\tmp\longtail";
//var longtailLibPath = Path.Combine(longtailBasePath, "lib");
//var longtailHeaderPath = Path.Combine(longtailBasePath, "src", "longtail.h");

//var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
//    .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly));
//    //.Concat(new[] { longtailHeaderPath })

//var includes = headerFiles.Select(h => $"#include \"{h}\"");
//var tempFile = Path.GetTempFileName();
//try
//{
//    File.WriteAllLines(tempFile, includes);
//    var process = Process.Start("gcc", $" -xc \"{tempFile}\" -E -P -o con:");
//    process.WaitForExit();
//}
//finally
//{
//    File.Delete(tempFile);
//}


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

