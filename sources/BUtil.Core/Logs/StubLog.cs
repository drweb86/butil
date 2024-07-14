namespace BUtil.Core.Logs;

public class StubLog : ILog
{
    public bool HasErrors => false;

    public void Close(bool isSuccess)
    {
    }

    public void Open()
    {
    }

    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
    {
    }

    public void WriteLine(LoggingEvent loggingEvent, string message)
    {
    }
}
