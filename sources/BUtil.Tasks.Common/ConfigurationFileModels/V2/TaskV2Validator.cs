using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Interop.Logs;
using BUtil.Interop.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BUtil.Core.ConfigurationFileModels.V2;

public static class TaskV2Validator
{
    public static bool TryValidate(
        TaskV2 task,
        bool writeMode,
        string? originalTaskName,
        [NotNullWhen(false)] out string? error)
    {
        var store = new TaskStore(new LocalFileSystem());
        if (!store.TryValidate(task.Name, originalTaskName, out error))
            return false;

        var memoryLog = new MemoryLog();

        if (!TaskProviderRegistry.TryVerify(memoryLog, task.Model, writeMode, out error))
        {
            error += Environment.NewLine + Environment.NewLine + memoryLog;
            return false;
        }

        error = null;
        return true;
    }
}
