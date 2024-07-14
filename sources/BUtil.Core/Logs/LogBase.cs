using System;
using System.Linq;

namespace BUtil.Core.Logs;

public abstract class LogBase : ILog
{
    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
    {
        consoleOutput
            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList()
            .ForEach(x => WriteLine(finishedSuccessfully ? LoggingEvent.Debug : LoggingEvent.Error, x));
    }

    public abstract void Open();
    public abstract void Close(bool isSuccess);
    public abstract void WriteLine(LoggingEvent loggingEvent, string message);
}
