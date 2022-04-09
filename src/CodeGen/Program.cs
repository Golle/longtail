using System.IO;
using System.Linq;
using CodeGen.Lexer;
using CodeGen.Logging;

using var _ = Logger.Start();

const string longtailBasePath = @"O:\tmp\longtail";
var longtailLibPath = Path.Combine(longtailBasePath, "lib");
var longtailHeaderPath = Path.Combine(longtailBasePath, "src", "longtail.h");

var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
    .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly))
    .Concat(new[] { longtailHeaderPath })
    //.Select(f => new Lexer(File.ReadAllText(f)).Process())
    .ToArray();

var firstFile = File.ReadAllText(headerFiles.First());

//var fileContents = firstFile;
////var fileContents = firstFile.Substring(0, 1000);
////var fileContents = "#define LONGTAIL_PREPROCESSOR_STR1_PRIVATE(x) #x\n";
var tokens = new Lexer(firstFile)
    .Process();




//Logger.Info(headerFiles.First());
//foreach (var token in tokens)
//{
//    if (token.Type == SyntaxTokenType.PreprocessorDirective)
//    {
//        Logger.Trace($"{token.PreProcessorDirective.ToString().PadRight(25)}{token.SingleValue}{token.Value}");
//    }
//    else
//    {
//        Logger.Trace($"{token.Type.ToString().PadRight(25)}{token.SingleValue}{token.Value}");
//    }
    
//}
