namespace BUtil.Core.Logs;

public interface ILog
{
    bool HasErrors { get; }
    void LogProcessOutput(string consoleOutput, bool finishedSuccessfully);
    void Open();
    void Close();
    void WriteLine(LoggingEvent loggingEvent, string message);
}
