using System;
using BUtil.Core.Events;

namespace BUtil.Core
{
    public interface IBackupModelStrategy : IDisposable
    {
        BackupEvents Events { get; }
        bool ErrorsOrWarningsRegistered { get; }
        void Run();
		void StopForcibly();
    }
}
