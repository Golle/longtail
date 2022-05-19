using System.Globalization;
using BuildTool.Logging;
using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal class ReadWriteCurrentVersion : IMiddleware<LongtailContext>
{
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        Logger.Info("Read current version");

        var versionFilePath = Path.Combine(context.BasePath, "LONGTAIL_VERSION");
        var currentVersion = await GetCurrentVersion(versionFilePath);

        if (currentVersion == null)
        {
            Logger.Trace("No current version file could be found.");
        }
        else
        {
            Logger.Info($"Current version is {currentVersion.Name} ({currentVersion.PublishedAt})");
        }

        context = await next(context with
        {
            CurrentVersion = currentVersion
        });
        if (context.Failed || context.NewVersion == null)
        {
            return context;
        }
        await WriteCurrentVersion(context.NewVersion, versionFilePath);
        return context;

        static async Task WriteCurrentVersion(LongtailVersion version, string versionFilePath) => await File.WriteAllLinesAsync(versionFilePath, new[] { version.Name, version.Tag, version.PublishedAt.ToString(CultureInfo.InvariantCulture) });

        static async Task<LongtailVersion?> GetCurrentVersion(string versionFilePath)
        {
            if (!File.Exists(versionFilePath))
            {
                return null;
            }
            var result = await File.ReadAllLinesAsync(versionFilePath);
            if (result.Length != 3)
            {
                Logger.Warning($"The {versionFilePath} seems to be corrupt. Expected 3 lines, but found {result.Length}. Please delete.");
                return null;
            }
            return new(result[0], result[1], DateTime.Parse(result[2]));
        }
    }
}