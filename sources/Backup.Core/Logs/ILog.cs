namespace BUtil.Core.Logs
{
    public interface ILog
    {
        bool IsOpened { get; }

        bool ErrorsOrWarningsRegistered { get; }
        void WriteLine(LoggingEvent loggingEvent, string message, params string[] arguments);
        void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully);
        void Open();
        void Close();
        void WriteLine(LoggingEvent loggingEvent, string message);
    }
}
