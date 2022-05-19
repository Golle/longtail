using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal record LongtailVersion(string Name, string Tag, DateTime PublishedAt);
internal record LongtailAsset(string Name, string DownloadUrl);
internal record LongtailContext(string BasePath, string LibraryPath) : Context
{
    public LongtailVersion? CurrentVersion { get; init; }
    public LongtailAsset[] Assets { get; init; } = Array.Empty<LongtailAsset>();
    public LongtailVersion? NewVersion { get; init; }
    public string[] LibraryDirectories { get; init; } = Array.Empty<string>();
    public string? GitRepoPath { get; init; }
}