using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using BuildTool.Logging;

using var _ = Logger.Start();

var basePath = FindFile(AppDomain.CurrentDomain.BaseDirectory, "Longtail.sln", 10)!;

HttpClient httpClient = new();

Logger.Info("Read current version");

var versionFilePath = Path.Combine(basePath, "LONGTAIL_VERSION");
var currentVersion = await GetCurrentVersion(versionFilePath);

if (currentVersion != null)
{
    Logger.Info($"Current version: {currentVersion.Name} - {currentVersion.PublishedAt}");
}
else
{
    Logger.Info("Longtail version has not been synced, downloading the latest.");
}


Logger.Info("Check for new versions of Longtail");
const string GithubAcceptHeaderValue = "application/vnd.github.v3+json";
const string BaseUrl = "https://api.github.com";
const string Owner = "DanEngelbrecht";
const string Repo = "longtail";
const string LongtailGithubPath = $"{BaseUrl}/repos/{Owner}/{Repo}/releases";

Logger.Info($"Checking for new versions at {LongtailGithubPath}");
httpClient.DefaultRequestHeaders.Accept.ParseAdd(GithubAcceptHeaderValue);
var request = new HttpRequestMessage(HttpMethod.Get, LongtailGithubPath)
{
    Headers =
    {
        { "user-agent", "longtail-code-gen" },
        { "accepts", GithubAcceptHeaderValue }
    }
};
var result = await httpClient.SendAsync(request);

var releases = await JsonSerializer.DeserializeAsync<GithubRelease[]>(await result.Content.ReadAsStreamAsync(), new JsonSerializerOptions
{
    PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
    PropertyNameCaseInsensitive = false
});

if (releases?.Length is null)
{
    Logger.Error("Failed to parse the versions");
    return -1;
}

var latestRelease = releases
    .Where(r => r.PublishedAt > (currentVersion?.PublishedAt ?? DateTime.MinValue))
    .MaxBy(r => r.PublishedAt);

if (latestRelease?.Assets == null)
{
    Logger.Info("No new versions available");
    return 0;
}

Logger.Info($"Downloading version: {latestRelease.Name}");

var expectedFileNames = new[] { "win32-x64.zip", "darwin-x64.zip", "linux-x64.zip" };
var assets = latestRelease
    .Assets
    .Where(a => expectedFileNames.Contains(a.Name))
    .ToArray();

if (assets.Length != 3)
{
    Logger.Error($"Failed to find the expected files: {string.Join(", ", expectedFileNames)}. Got: {string.Join(", ", assets.Select(a => a.Name))}");
    return -1;
}

var tempDir = Path.Combine(Path.GetTempPath(), "longtail-lib");
if (Directory.Exists(tempDir))
{
    Logger.Info($"Temp directory alreayd exists, deleting.");
    Directory.Delete(tempDir, true);
}
Directory.CreateDirectory(tempDir);

try
{
    Logger.Info($"Using {tempDir} for temporary storage.");
    var destination = Path.Combine(basePath, "lib");
    Logger.Info($"Longtail library path: {destination}");
    DeleteOldFiles(destination);
    foreach (var asset in assets)
    {
        await DownloadAndExtract(asset, tempDir);
        Logger.Info($"Unzipped {asset.Name} into {tempDir}");
        CopyLibraryFiles(asset.Name, tempDir, destination);
    }
    await WriteCurrentVersion(new CurrentVersion(latestRelease.Name, latestRelease.PublishedAt), versionFilePath);
}
finally
{
    Logger.Info($"Deleting temporary directory {tempDir}");
    Directory.Delete(tempDir, true);
}

return 0;

static void DeleteOldFiles(string destination)
{
    foreach (var file in Directory.EnumerateFiles(destination))
    {
        Logger.Info($"Deleting old file: {Path.GetFileName(file)}");
        File.Delete(file);
    }
}

static void CopyLibraryFiles(string zipFileName, string source, string destination)
{
    var name = Path.GetFileNameWithoutExtension(zipFileName);
    var folder = Path.Combine(source, $"dist-{name}");
    var files = Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly)
        .Where(f => f.EndsWith(".so") || f.EndsWith(".pdb") || f.EndsWith(".dll"));

    foreach (var file in files)
    {
        var fileName = Path.GetFileName(file);
        Logger.Info($"Copy {fileName} to {destination}");
        File.Copy(file, Path.Combine(destination, fileName), true);
    }
}

static async Task DownloadAndExtract(GithubAsset asset, string tempDir)
{
    using var httpClient = new HttpClient(); // NOTE(Jens): this is bad practice, should only have a single instance.
    var stream = await httpClient.GetStreamAsync(asset.BrowserDownloadUrl);
    var zipFile = Path.Combine(tempDir, asset.Name);
    {
        await using var file = File.OpenWrite(zipFile);
        file.SetLength(0);
        await stream.CopyToAsync(file);
    }
    var unzipDestination = Path.Combine(tempDir);
    ZipFile.ExtractToDirectory(zipFile, unzipDestination, true);
}

static async Task<CurrentVersion?> GetCurrentVersion(string versionFilePath)
{
    if (!File.Exists(versionFilePath))
    {
        return null;
    }
    var result = await File.ReadAllLinesAsync(versionFilePath);
    if (result.Length != 2)
    {
        Logger.Warning($"The {versionFilePath} seems to be corrupt. Expected 2 lines, but found {result.Length}. Please delete.");
        return null;
    }
    return new(result[0], DateTime.Parse(result[1]));
}

static async Task WriteCurrentVersion(CurrentVersion currentVersion, string path)
{
    await File.WriteAllLinesAsync(path, new[] { currentVersion.Name, currentVersion.PublishedAt.ToString(CultureInfo.InvariantCulture) });
}

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

internal record GithubRelease(string Name, DateTime PublishedAt, bool PreRelease, string Url, GithubAsset[]? Assets);
internal record GithubAsset(string Name, string ContentType, string BrowserDownloadUrl, long Size);
internal record CurrentVersion(string Name, DateTime PublishedAt);

internal class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        Span<char> buffer = stackalloc char[name.Length * 2];
        var charCount = 0;

        for (var i = 0; i < name.Length; i++)
        {
            var character = name[i];
            if (i != 0 && char.IsUpper(character))
            {
                buffer[charCount++] = '_';
            }

            buffer[charCount++] = char.ToLower(character);
        }

        return new string(buffer[..charCount]);
    }
}
