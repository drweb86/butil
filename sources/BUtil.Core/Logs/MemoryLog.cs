using System.Text;

namespace BUtil.Core.Logs;

public class MemoryLog : ILog
{
    public void Close(bool isSuccess)
    {
    }

    public void Open()
    {
    }

    private readonly StringBuilder _log = new();

    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
    {
        _log.AppendLine(consoleOutput);
        _log.AppendLine(finishedSuccessfully.ToString());
    }

    public void WriteLine(LoggingEvent loggingEvent, string message)
    {
        _log
            .Append(loggingEvent.ToString())
            .Append(' ')
            .AppendLine(message);
    }

    public override string ToString()
    {
        return _log.ToString();
    }
}
