using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeGen.Logging;

namespace CodeGen;

internal class LongtailFileReader
{
    private readonly string _longtailPath;

    public LongtailFileReader(string longtailPath)
    {
        _longtailPath = longtailPath;
    }

    public IEnumerable<string> EnumerateAllHeaders(params (string Value, string? NewValue)[] replacements)
    {
        Logger.Info($"Reading header files in {_longtailPath}");
        var longtailLibPath = Path.Combine(_longtailPath, "lib");
        var longtailHeader = Path.Combine(_longtailPath, "src", "longtail.h");

        var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
            .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly))
            .ToArray();

        
        Logger.Trace($"Read contents of: {longtailHeader}");
        yield return ReplaceContentes(File.ReadAllText(longtailHeader));
        foreach (var headerFile in headerFiles)
        {
            Logger.Trace($"Read contents of: {headerFile}");
            yield return ReplaceContentes(File.ReadAllText(headerFile));
        }

        string ReplaceContentes(string input)
        {
            // NOTE(Jens): this is really slow, allocating a new string for each replacement
            foreach (var (value, newValue) in replacements)
            {
                input = input.Replace(value, newValue);
            }
            return input;
        }
    }

}