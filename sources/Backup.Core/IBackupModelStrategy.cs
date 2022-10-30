using System;
using BUtil.Core.Logs;

namespace BUtil.Core
{
    public interface IBackupModelStrategy : IDisposable
    {
        LogBase Log { get; }

        event EventHandler NotificationEventHandler;
        BackupFinished BackupFinished { get; set; }
        CriticalErrorOccur CriticalErrorOccur { get; set; }
        bool ErrorsOrWarningsRegistered { get; }
        void Run();
		void StopForcibly();
    }
}
