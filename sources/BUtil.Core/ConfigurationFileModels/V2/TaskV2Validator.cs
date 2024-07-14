using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.TasksTree;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BUtil.Core.ConfigurationFileModels.V2;

public static class TaskV2Validator
{
    public static bool TryValidate(TaskV2 task, bool writeMode, [NotNullWhen(false)] out string? error)
    {
        if (!new TaskV2StoreService().TryValidate(task.Name, out error))
        {
            return false;
        }

        var memoryLog = new MemoryLog();

        if (!RootTaskFactory.TryVerify(memoryLog, task.Model, writeMode, out error))
        {
            error += Environment.NewLine + Environment.NewLine + memoryLog;
            return false;
        }

        error = null;
        return true;
    }
}
