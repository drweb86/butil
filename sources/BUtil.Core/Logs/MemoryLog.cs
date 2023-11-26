using System.Text;

namespace BUtil.Core.Logs
{
    public class MemoryLog : ILog
    {
        public bool HasErrors => false;

        public void Close()
        {
        }

        public void Open()
        {
        }

        private StringBuilder _log = new StringBuilder();

        public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
        {
            _log.AppendLine(consoleOutput);
            _log.AppendLine(finishedSuccessfully.ToString());
        }

        public void WriteLine(LoggingEvent loggingEvent, string message)
        {
            _log.Append(loggingEvent.ToString());
            _log.Append(" ");
            _log.AppendLine(message);
        }

        public override string ToString()
        {
            return _log.ToString();
        }
    }
}
