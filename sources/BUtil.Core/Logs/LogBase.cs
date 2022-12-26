using System;
using System.Linq;

namespace BUtil.Core.Logs
{
    public abstract class LogBase: ILog
    {
        private bool _errorsOrWarningsRegistered;

        public bool ErrorsOrWarningsRegistered => _errorsOrWarningsRegistered;
        public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
        {
            consoleOutput
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(x => WriteLine(finishedSuccessfully ? LoggingEvent.Debug : LoggingEvent.Error, x));
        }

        protected void PreprocessLoggingInformation(LoggingEvent loggingEvent)
        {
        	if (loggingEvent == LoggingEvent.Error)
        		_errorsOrWarningsRegistered = true;
        }

        public abstract void Open();
        public abstract void Close();
        public abstract void WriteLine(LoggingEvent loggingEvent, string message);
    }
}
