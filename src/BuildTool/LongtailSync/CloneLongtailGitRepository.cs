using System.Diagnostics;
using BuildTool.Logging;
using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal class CloneLongtailGitRepository : IMiddleware<LongtailContext>
{
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        const string RepoUrl = "https://github.com/DanEngelbrecht/longtail.git";
        var repoPath = Path.Combine(context.BasePath, "tmp", "repo");
        if (Directory.Exists(repoPath))
        {
            RemoveReadOnlyAttributes(Path.Combine(repoPath, ".git"));
            Logger.Info($"Deleting {repoPath}");
            Directory.Delete(repoPath, true);
        }
        Directory.CreateDirectory(repoPath);

        Logger.Info($"Cloning longtail repo with tag {context.NewVersion!.Tag}");
        var arguments = $"clone --branch {context.NewVersion!.Tag} --depth 1 {RepoUrl} .";
        var process = Process.Start(new ProcessStartInfo("git", arguments)
        {
            WorkingDirectory = repoPath,
            CreateNoWindow = true
        });

        if (process == null)
        {
            return context with { Failed = true, Reason = "Failed to spawn the process." };
        }
        process.WaitForExit((int)TimeSpan.FromMinutes(2).TotalMilliseconds);

        if (!process.HasExited)
        {
            Logger.Error("Timeout reached for git clone.");
            process.Kill(true);
            return context with { Failed = true, Reason = $"Failed to clone {RepoUrl}" };
        }

        if (process.ExitCode != 0)
        {
            return context with { Failed = true, Reason = $"Failed to clone {RepoUrl} with exit code: {process.ExitCode}" };
        }

        return await next(context with
        {
            GitRepoPath = repoPath
        });


        static void RemoveReadOnlyAttributes(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                var info = new FileInfo(file);
                if ((info.Attributes & FileAttributes.ReadOnly) != 0)
                {
                    Logger.Trace($"Removing read only attribute on {Path.GetFileName(file)}");
                    File.SetAttributes(file, FileAttributes.Normal);
                }
            }
        }
    }
}