#nullable enable

using BUtil.Core.Misc;
using System;

namespace BUtil.Core.TasksTree.Core;

public static class BuTaskExtensions
{
    public static TTask EnsureSuccess<TTask>(this TTask task)
        where TTask: BuTask
    {
        task.EnsureNotNull();

        if (!task.IsSuccess)
            throw new InvalidOperationException($"{task.Title} failed!");

        return task;
    }
}
