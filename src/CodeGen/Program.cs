using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeGen.Logging;using CodeGen.Parser;
using CodeGen.Tokenizer;

using var _ = Logger.Start();

const string longtailBasePath = @"O:\tmp\longtail";
var longtailLibPath = Path.Combine(longtailBasePath, "lib");
var longtailHeaderPath = Path.Combine(longtailBasePath, "src", "longtail.h");

var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
    .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly))
    .Concat(new[] { longtailHeaderPath })
    //.Select(f => new Lexer(File.ReadAllText(f)).Process())
    .ToArray();

var firstFilename = headerFiles.First();
var firstFileContents = File.ReadAllText(firstFilename);

//var fileContents = firstFile;
////var fileContents = firstFile.Substring(0, 1000);
////var fileContents = "#define LONGTAIL_PREPROCESSOR_STR1_PRIVATE(x) #x\n";

Logger.Info(firstFilename);

var lookupTable = new Dictionary<string, TokenType>()
{
    { "LONGTAIL_EXPORT", TokenType.DllExport }
};

var tokens = new Tokenizer(true, lookupTable)
    .Tokenize(firstFileContents)
    .ToArray()
    ;


var node = new LongtailParser()
    .Parse(tokens);

//
//foreach (var token in tokens)
//{
//    Logger.Trace($"{token.Type.ToString().PadRight(25)}{token.Value.PadRight(50)}{token.Line}:{token.Column}");

//}
