namespace BUtil.Core.Logs;

public interface ILog
{
    void LogProcessOutput(string consoleOutput, bool finishedSuccessfully);
    void Open();
    void Close(bool isSuccess);
    void WriteLine(LoggingEvent loggingEvent, string message);
}
