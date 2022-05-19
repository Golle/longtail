using BuildTool.Logging;
using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal class DeleteLibraryFiles : IMiddleware<LongtailContext>
{
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        foreach (var file in Directory.EnumerateFiles(context.LibraryPath))
        {
            Logger.Info($"Deleting old file: {Path.GetFileName(file)}");
            File.Delete(file);
        }

        return await next(context);
    }
}