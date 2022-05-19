using System.Diagnostics;
using System.IO.Compression;
using BuildTool.Logging;
using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal class DownloadLongtailLibrary : IMiddleware<LongtailContext>
{
    private static readonly HttpClient Client = new();
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        var downloadDir = Path.Combine(context.BasePath, "tmp", "downloads");

        if (!Directory.Exists(downloadDir))
        {
            Logger.Trace($"Creating folder: {downloadDir}");
            Directory.CreateDirectory(downloadDir);
        }
        Logger.Trace($"Download {context.Assets.Length} assets.");
        var tasks = context.Assets.Select(a => DownloadAndExtract(a, downloadDir));
        await Task.WhenAll(tasks);
        Logger.Trace("Download complete.");

        var directories = Directory.GetDirectories(downloadDir, "dist-*", SearchOption.TopDirectoryOnly);
        if (directories.Length != context.Assets.Length)
        {
            return context with { Failed = true, Reason = $"Expected {context.Assets.Length} directories but found {directories.Length} directories." };
        }

        return await next(context with
        {
            LibraryDirectories = directories
        });

        static async Task DownloadAndExtract(LongtailAsset asset, string destination)
        {
            var timer = Stopwatch.StartNew();
            Logger.Trace($"Download {asset.Name}");
            var stream = await Client.GetStreamAsync(asset.DownloadUrl);
            var zipFile = Path.Combine(destination, asset.Name);
            {
                await using var file = File.OpenWrite(zipFile);
                file.SetLength(0);
                await stream.CopyToAsync(file);
            }
            Logger.Trace($"Unzip {asset.Name}");
            ZipFile.ExtractToDirectory(zipFile, destination, true);
            Logger.Trace($"Completed {asset.Name} in {timer.Elapsed.TotalMilliseconds:0.##} ms");

            
        }
    }
}