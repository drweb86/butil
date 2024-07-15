using System;
using System.Linq;

namespace BUtil.Core.Logs;

public abstract class LogBase : ILog
{
    private static readonly char[] _separator = ['\n', '\r'];

    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
    {
        consoleOutput
            .Split(_separator, StringSplitOptions.RemoveEmptyEntries)
            .ToList()
            .ForEach(x => WriteLine(finishedSuccessfully ? LoggingEvent.Debug : LoggingEvent.Error, x));
    }

    public abstract void Open();
    public abstract void Close(bool isSuccess);
    public abstract void WriteLine(LoggingEvent loggingEvent, string message);
}
