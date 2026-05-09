using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Interop.Logs;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BUtil.Core.TasksTree;

public static class RootTaskFactory
{
    public static BuTask Create(ILog log, TaskV2 task, TaskEvents events, Action<string?> onGetLastMinuteMessage)
    {
        return TaskProviderRegistry.Create(log, task, events, onGetLastMinuteMessage);
    }

    public static bool TryVerify(ILog log, ITaskModelOptionsV2 options, bool writeMode, [NotNullWhen(false)] out string? error)
    {
        return TaskProviderRegistry.TryVerify(log, options, writeMode, out error);
    }
}
