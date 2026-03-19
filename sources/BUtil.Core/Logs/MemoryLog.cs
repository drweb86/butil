using System.Text;

namespace BUtil.Core.Logs;

public class MemoryLog : ILog
{
    private readonly object _sync = new();

    public void Close(bool isSuccess)
    {
    }

    public void Open()
    {
    }

    private readonly StringBuilder _log = new();

    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
    {
        lock (_sync)
        {
            _log.AppendLine(consoleOutput);
            _log.AppendLine(finishedSuccessfully.ToString());
        }
    }

    public void WriteLine(LoggingEvent loggingEvent, string message)
    {
        lock (_sync)
        {
            _log
                .Append(loggingEvent.ToString())
                .Append(' ')
                .AppendLine(message);
        }
    }

    public override string ToString()
    {
        lock (_sync)
        {
            return _log.ToString();
        }
    }
}
