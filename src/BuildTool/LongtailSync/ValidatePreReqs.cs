using System.Diagnostics;
using BuildTool.Logging;
using BuildTool.Pipeline;

namespace BuildTool.LongtailSync;

internal class ValidatePreReqs : IMiddleware<LongtailContext>
{
    public async Task<LongtailContext> OnInvoke(LongtailContext context, ContextDelegate<LongtailContext> next)
    {
        if (!TryCommand("git", "--version"))
        {
            return context with { Failed = true, Reason = "Failed to run git --version, this tool requires that git exist in the PATH." };
        }

        if (!TryCommand("dotnet", "--version"))
        {
            return context with { Failed = true, Reason = "Failed to run dotnet --version, this tool requires that dotnet exist in the PATH." };
        }

        return await next(context);
        static bool TryCommand(string command, string arguments)
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo(command, arguments)
                {
                    CreateNoWindow = true
                });
                const int milliseconds = 5000;
                process!.WaitForExit(milliseconds);
                if (process.HasExited)
                {
                    return process.ExitCode == 0;
                }
                process.Kill(true);
                Logger.Warning($"Command {command} took more than {milliseconds} ms to execute.");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Command {command} threw a {e.GetType().Name} with message {e.Message}");
                return false;
            }
        }
    }
}