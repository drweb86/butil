using BUtil.Core.Logs;
using System.Collections.Generic;

namespace butilc;

class ChainLog(string taskName) : ILog
{
    private readonly List<LogBase> _logs = [
            new FileLog(taskName),
            new ConsoleLog()
        ];

    public void Open() => _logs.ForEach(x => x.Open());
    public void Close(bool isSuccess) => _logs.ForEach(x => x.Close(isSuccess));
    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
        => _logs.ForEach(x => x.LogProcessOutput(consoleOutput, finishedSuccessfully));
    public void WriteLine(LoggingEvent loggingEvent, string message)
        => _logs.ForEach(x => x.WriteLine(loggingEvent, message));
}