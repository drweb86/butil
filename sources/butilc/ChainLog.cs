using BUtil.Core.Logs;
using System.Collections.Generic;

namespace butilc;

class ChainLog(string taskName) : ILog
{
    private readonly List<LogBase> _logs = [
            new FileLog(taskName),
            new ConsoleLog()
        ];

    public void Open() => ForEachSafe(x => x.Open());
    public void Close(bool isSuccess) => ForEachSafe(x => x.Close(isSuccess));
    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
        => ForEachSafe(x => x.LogProcessOutput(consoleOutput, finishedSuccessfully));
    public void WriteLine(LoggingEvent loggingEvent, string message)
        => ForEachSafe(x => x.WriteLine(loggingEvent, message));

    private void ForEachSafe(System.Action<LogBase> action)
    {
        foreach (var log in _logs)
        {
            try
            {
                action(log);
            }
            catch
            {
                // Keep best-effort fan-out logging.
            }
        }
    }
}