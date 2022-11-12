namespace BUtil.Core.Logs
{
    public class StubLog : ILog
    {
        public bool IsOpened { get; private set; }

        public bool ErrorsOrWarningsRegistered => false;

        public void Close()
        {
            IsOpened= false;
        }

        public void Open()
        {
            IsOpened= true;
        }

        public void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully)
        {
        }

        public void WriteLine(LoggingEvent loggingEvent, string message, params string[] arguments)
        {
        }

        public void WriteLine(LoggingEvent loggingEvent, string message)
        {
        }
    }
}
