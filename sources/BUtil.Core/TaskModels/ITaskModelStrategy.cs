using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.BackupModels;

public interface ITaskModelStrategy
{
    BuTask GetTask(TaskEvents events);
}
