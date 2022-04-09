using System.IO;
using System.Linq;
using CodeGen.Logging;

using var _ = Logger.Start();

const string longtailBasePath = @"O:\tmp\longtail";
var longtailLibPath = Path.Combine(longtailBasePath, "lib");
var longtailHeaderPath = Path.Combine(longtailBasePath, "src", "longtail.h");

var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
    .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly))
    .Concat(new[] { longtailHeaderPath });


foreach (var file in headerFiles)
{
    Logger.Info(file);
}



