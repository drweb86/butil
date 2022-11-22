using BUtil.Core.Logs;
using BUtil.Core.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BUtil.ConsoleBackup
{
    class ChainLog : ILog
    {
        private readonly IEnumerable<LogBase> _logs;

        public ChainLog(ProgramOptions programOptions)
        {
            var logs = new List<LogBase>();
            _logs = logs;
            logs.Add(new FileLog(programOptions.LogsFolder, true));
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

        public void ProcessPackerMessage(string consoleOutput, bool finishedSuccessfully)
        {
            foreach (var log in _logs)
                log.ProcessPackerMessage(consoleOutput, finishedSuccessfully);
        }


        public void WriteLine(LoggingEvent loggingEvent, string message)
        {
            foreach (var log in _logs)
                log.WriteLine(loggingEvent, message);
        }
    }
}
