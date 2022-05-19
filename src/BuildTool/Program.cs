using BuildTool.Logging;
using BuildTool.LongtailSync;using BuildTool.Pipeline;

using var _ = Logger.Start();

var basePath = FindFile(AppDomain.CurrentDomain.BaseDirectory, "Longtail.sln", 10)!;
var libraryPath = Path.Combine(basePath, "lib");

var longtailResult = await new PipelineBuilder<LongtailContext>()
    .With<ReadWriteCurrentVersion>()
    .With<GetLongtailReleases>()
    .With<DownloadLongtailLibrary>()
    .With<DeleteLibraryFiles>()
    .With<CopyLibraryFiles>()
    .With<CloneLongtailGitRepository>()
    .Build()
    .Invoke(new LongtailContext(basePath, libraryPath));

if (longtailResult.Failed)
{
    Logger.Error($"Longtail sync failed with reason: {longtailResult.Reason}");

    return -1;
}

Logger.Info("Success!");
return 0;


static string? FindFile(string? currentPath, string name, int parents = 6)
{
    if (currentPath == null || parents == 0)
    {
        return null;
    }
    var fullPath = Path.Combine(currentPath, name);
    if (File.Exists(fullPath))
    {
        return currentPath;
    }
    return FindFile(Directory.GetParent(currentPath)?.FullName, name, parents - 1);
}
