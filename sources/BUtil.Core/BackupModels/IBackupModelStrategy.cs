using System;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.BackupModels
{
    public interface IBackupModelStrategy
    {
        BuTask GetTask(BackupEvents events);
    }
}
