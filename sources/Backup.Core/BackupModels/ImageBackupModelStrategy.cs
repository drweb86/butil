using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Events;
using BUtil.Core.TasksTree;

namespace BUtil.Core.BackupModels
{
    class IncrementalBackupModelStrategy : IBackupModelStrategy
    {
        readonly ProgramOptions _options;
        private readonly IncrementalBackupModelOptions _modelOptions;
        readonly LogBase _log;
        readonly BackupTask _task;

        public IncrementalBackupModelStrategy(LogBase openedLog, BackupTask task, IncrementalBackupModelOptions modelOptions, ProgramOptions options)
        {
            _modelOptions = modelOptions;
            _log = openedLog;
            _task = task;
            _options = options;
            HtmlLogFormatter.IncludeLoggingEventPrefixes = options.LoggingLevel == LogLevel.Support;
        }

        public BuTask GetTask(BackupEvents events)
        {
            var task = new IncrementalBackupTask(_log, events, _task, _options, _modelOptions);
            return task;
        }
    }
}
