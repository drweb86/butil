using BUtil.Core.Logs;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.ConsoleBackup
{
    class ChainLog : ILog
    {
        private readonly List<LogBase> _logs;

        public ChainLog(string taskName)
        {
            _logs = new () {
                new FileLog(taskName),
                new ConsoleLog()
            };
        }

        public bool HasErrors => _logs.Any(x => x.HasErrors);
        public void Open() => _logs.ForEach(x => x.Open());
        public void Close() => _logs.ForEach(x => x.Close());
        public void LogProcessOutput(string consoleOutput, bool finishedSuccessfully)
            => _logs.ForEach(x => x.LogProcessOutput(consoleOutput, finishedSuccessfully));
        public void WriteLine(LoggingEvent loggingEvent, string message)
            => _logs.ForEach(x => x.WriteLine(loggingEvent, message));
    }
}
