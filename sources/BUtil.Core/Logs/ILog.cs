namespace BUtil.Core.Logs
{
    public interface ILog
    {
        bool ErrorsOrWarningsRegistered { get; }
        void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully);
        void Open();
        void Close();
        void WriteLine(LoggingEvent loggingEvent, string message);
    }
}
