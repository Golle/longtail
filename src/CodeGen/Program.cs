using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeGen.Logging;


using var _ = Logger.Start();

const string longtailBasePath = @"O:\tmp\longtail";
var longtailLibPath = Path.Combine(longtailBasePath, "lib");
var longtailHeaderPath = Path.Combine(longtailBasePath, "src", "longtail.h");

var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
    .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly))
    .Concat(new[] { longtailHeaderPath })
    .ToArray();


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

