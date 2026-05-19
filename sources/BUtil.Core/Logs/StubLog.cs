using BUtil.Interop.Logs;

namespace BUtil.Core.Logs;

internal sealed class StubLog : ILog
{
    public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
    {
    }

    public void Open()
    {
    }

    public void Close(bool isSuccess)
    {
    }

    public void WriteLine(LoggingEvent loggingEvent, string message)
    {
    }
}
