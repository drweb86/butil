namespace BUtil.Core.Logs
{
    public class StubLog : ILog
    {
        public bool ErrorsOrWarningsRegistered => false;

        public void Close()
        {
        }

        public void Open()
        {
        }

        public void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully)
        {
        }

        public void WriteLine(LoggingEvent loggingEvent, string message)
        {
        }
    }
}
