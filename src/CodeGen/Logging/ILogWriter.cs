using System;
using System.Threading.Tasks;

namespace CodeGen.Logging;

public interface ILogWriter
{
    ValueTask OnStartup() => ValueTask.CompletedTask;
    ValueTask OnMessage(LogMessage logMessage);
    ValueTask OnDispose() => ValueTask.CompletedTask;
    ValueTask OnError(Exception exception);
}