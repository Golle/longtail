using BuildTool.Logging;
using BuildTool.Pipeline;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BuildTool.LongtailSync;

internal class GetLongtailReleases : IMiddleware<LongtailContext>
{
    private static readonly HttpClient Client = new();
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        const string GithubAcceptHeaderValue = "application/vnd.github.v3+json";
        const string BaseUrl = "https://api.github.com";
        const string Owner = "DanEngelbrecht";
        const string Repo = "longtail";
        const string LongtailGithubPath = $"{BaseUrl}/repos/{Owner}/{Repo}/releases";

        Logger.Info($"Checking for new versions at {LongtailGithubPath}");

        Client.DefaultRequestHeaders.Accept.ParseAdd(GithubAcceptHeaderValue);
        var request = new HttpRequestMessage(HttpMethod.Get, LongtailGithubPath)
        {
            Headers =
            {
                { "user-agent", "longtail-code-gen" },
                { "accepts", GithubAcceptHeaderValue }
            }
        };
        var result = await Client.SendAsync(request);
        if (!result.IsSuccessStatusCode)
        {
            return context with { Failed = true, Reason = $"Getting version from Github failed with code: {result.StatusCode}" };
        }

        var releases = await JsonSerializer.DeserializeAsync<GithubRelease[]>(await result.Content.ReadAsStreamAsync(), new JsonSerializerOptions());

        if (releases?.Length is null)
        {
            return context with { Failed = true, Reason = "Failed to parse the versions from github" };
        }

        var latestRelease = releases
            .Where(r => !r.PreRelease) // Ignore pre-releases
            .MaxBy(r => r.PublishedAt);
        if (latestRelease?.Assets == null)
        {
            return context with { Failed = true, Reason = "Failed to find any releases." };
        }

        if (latestRelease.PublishedAt == context.CurrentVersion?.PublishedAt && latestRelease.Name == context.CurrentVersion?.Name)
        {
            Logger.Info($"We're already on the latest version, aborting. ({latestRelease.Name} - {latestRelease.PublishedAt})");
            return context;
        }
        Logger.Info($"Found a new version: {latestRelease.Name} ({latestRelease.PublishedAt})");

        var expectedFileNames = new[] { "win32-x64.zip", "darwin-x64.zip", "linux-x64.zip" };
        var assets = latestRelease
            .Assets
            .Where(a => expectedFileNames.Contains(a.Name))
            .Select(a => new LongtailAsset(Path.GetFileNameWithoutExtension(a.Name), a.BrowserDownloadUrl))
            .ToArray();

        if (assets.Length != 3)
        {
            return context with { Failed = true, Reason = $"Failed to find the expected files: {string.Join(", ", expectedFileNames)}. Got: {string.Join(", ", assets.Select(a => a.Name))}" };
        }

        return await next(context with
        {
            Assets = assets,
            NewVersion = new LongtailVersion(latestRelease.Name, latestRelease.TagName, latestRelease.PublishedAt)
        });
    }

    internal record GithubRelease(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("tag_name")] string TagName,
        [property: JsonPropertyName("published_at")] DateTime PublishedAt,
        [property: JsonPropertyName("prerelease")] bool PreRelease,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("assets")] GithubAsset[]? Assets);

    internal record GithubAsset(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("content_type")] string ContentType,
        [property: JsonPropertyName("browser_download_url")] string BrowserDownloadUrl,
        [property: JsonPropertyName("size")] long Size);
}