using BuildTool.Logging;
using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal class CopyLibraryFiles : IMiddleware<LongtailContext>
{
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        foreach (var source in context.LibraryDirectories)
        {
            CopyFiles(source, context.LibraryPath);
        }

        static void CopyFiles(string source, string destination)
        {
            var files = Directory.EnumerateFiles(source, "*.*")
                .Where(f => f.EndsWith(".dll") || f.EndsWith(".so") || f.EndsWith(".pdb"));

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destinationFileName = Path.Combine(destination, fileName);
                Logger.Trace($"Copy {fileName} to {destinationFileName}");
                File.Copy(file, destinationFileName, true);
            }
        }

        return await next(context);
    }
}