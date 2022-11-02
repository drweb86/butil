using System;
using System.Collections.Generic;
using BUtil.Core.Events;

namespace BUtil.Core.BackupModels
{
    public interface IBackupModelStrategy : IDisposable
    {
        Dictionary<object, string> CustomJobs { get; }
        BackupEvents Events { get; }
        bool ErrorsOrWarningsRegistered { get; }
        void Run();
        void StopForcibly();
    }
}
