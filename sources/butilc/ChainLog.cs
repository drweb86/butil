using BUtil.Core.Logs;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.ConsoleBackup
{
    class ChainLog : ILog
    {
        private readonly IEnumerable<LogBase> _logs;

        public ChainLog(string taskName)
        {
            var logs = new List<LogBase>();
            _logs = logs;
            logs.Add(new FileLog(taskName));
            logs.Add(new ConsoleLog());
        }

        public bool ErrorsOrWarningsRegistered => _logs.Any(x => x.ErrorsOrWarningsRegistered);

        public void Close()
        {
            foreach (var log in _logs)
                log.Close();
        }

        public void Open()
        {
            foreach (var log in _logs)
                log.Open();
        }

        public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
        {
            foreach (var log in _logs)
                log.LogProcessOutput(consoleOutput, finishedSuccessfully);
        }


        public void WriteLine(LoggingEvent loggingEvent, string message)
        {
            foreach (var log in _logs)
                log.WriteLine(loggingEvent, message);
        }
    }
}
