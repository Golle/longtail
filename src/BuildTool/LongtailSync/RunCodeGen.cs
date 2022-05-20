using System.Diagnostics;
using BuildTool.Logging;
using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal class RunCodeGen : IMiddleware<LongtailContext>
{
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        Logger.Info("Generate longtail code");
        const string dotnetArguments = "run --project src/CodeGen -c Release --";

        var longtailGeneratedPath = Path.Combine(context.BasePath, "src", "Longtail", "Generated");
        var longtailRepo = context.GitRepoPath!;

        var arguments = $"\"{longtailRepo}\" \"{longtailGeneratedPath}\"";

        Logger.Trace($"Run command dotnet {dotnetArguments} {arguments}");

        var process = Process.Start(new ProcessStartInfo("dotnet", $"{dotnetArguments} {arguments}")
        {
            WorkingDirectory = context.BasePath,
            CreateNoWindow = !context.ShowCodeGenOutput
        });
        if (process == null)
        {
            return context with { Failed = true, Reason = "Process.Start returned null." };
        }
        process.WaitForExit((int)TimeSpan.FromMinutes(5).TotalMilliseconds);

        Logger.Info($"Completed code gen with exit code: {process.ExitCode}");

        if (process.ExitCode != 0)
        {
            return context with { Failed = true, Reason = $"Code Gen failed with exit code {process.ExitCode}" };
        }

        return await next(context);
    }
}