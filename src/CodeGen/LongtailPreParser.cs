using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CodeGen.Logging;

namespace CodeGen;

internal class LongtailPreParser
{
    private readonly string _longtailPath;
    public LongtailPreParser(string longtailPath)
    {
        _longtailPath = longtailPath;
    }

    public string Run()
    {
        Logger.Info($"Run preprocess on header files in {_longtailPath}");
        var longtailLibPath = Path.Combine(_longtailPath, "lib");
        var longtailHeader = Path.Combine(_longtailPath, "src", "longtail.h");

        var headerFiles = Directory.GetDirectories(longtailLibPath, "*", SearchOption.TopDirectoryOnly)
            .SelectMany(lib => Directory.GetFiles(lib, "*.h", SearchOption.TopDirectoryOnly));


        var includes = headerFiles.Select(h => $"#include \"{h}\"");
        var tempFile = Path.GetTempFileName();
        var outTempFile = Path.GetTempFileName();
        
        try
        {
            File.WriteAllLines(tempFile, includes);
            using var process = new Process
            {
                //StartInfo = new ProcessStartInfo("gcc", $" -xc \"{tempFile}\" -E -P -o con:")
                StartInfo = new ProcessStartInfo("cmd", $"/c gcc -nostdinc -xc \"{tempFile}\" -E -P -o \"{outTempFile}\"")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            StringBuilder stdErr = new();

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    stdErr.AppendLine(args.Data);
                }
            };

            process.Start();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Logger.Error($"gcc failed with exit code {process.ExitCode}");
                Logger.Error($"stderr: {stdErr}");
            }
            return File.ReadAllText(outTempFile);
        }
        finally
        {
            File.Delete(tempFile);
            File.Delete(outTempFile);
        }
    }
}