using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Interop.Logs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BUtil.Core.TasksTree;

/// <summary>
/// Registry for task providers. Host applications register built-in task plugins at startup;
/// external plugins register via <see cref="ITaskPlugin"/> implementations.
/// </summary>
public static class TaskProviderRegistry
{
    private sealed record ProviderEntry(
        Type ModelType,
        Func<ILog, TaskV2, TaskEvents, Action<string?>, BuTask> Factory,
        Func<ILog, ITaskModelOptionsV2, bool, string?> Verifier);

    private static readonly List<ProviderEntry> Entries = [];
    private static readonly object EntriesLock = new();

    /// <summary>
    /// Registers a task provider for the given model type.
    /// </summary>
    public static void Register<TModel>(
        Func<ILog, TaskV2, TaskEvents, Action<string?>, BuTask> factory,
        Func<ILog, TModel, bool, string?> verifier)
        where TModel : class, ITaskModelOptionsV2
    {
        lock (EntriesLock)
            Entries.Add(new ProviderEntry(typeof(TModel), factory, (log, opts, write) => verifier(log, (TModel)opts, write)));
    }

    /// <summary>
    /// Creates a root task for the given task configuration.
    /// </summary>
    public static BuTask Create(ILog log, TaskV2 task, TaskEvents events, Action<string?> onGetLastMinuteMessage)
    {
        lock (EntriesLock)
        {
            var entry = Entries.FirstOrDefault(e => task.Model.GetType() == e.ModelType)
                ?? throw new ArgumentOutOfRangeException(nameof(task), $"No task provider registered for model type '{task.Model.GetType().Name}'.");
            return entry.Factory(log, task, events, onGetLastMinuteMessage);
        }
    }

    /// <summary>
    /// Validates task options. Returns true if valid; false with an error message otherwise.
    /// </summary>
    public static bool TryVerify(ILog log, ITaskModelOptionsV2 options, bool writeMode, [NotNullWhen(false)] out string? error)
    {
        lock (EntriesLock)
        {
            var entry = Entries.FirstOrDefault(e => options.GetType() == e.ModelType)
                ?? throw new ArgumentOutOfRangeException(nameof(options), $"No task provider registered for model type '{options.GetType().Name}'.");
            error = entry.Verifier(log, options, writeMode);
            return error == null;
        }
    }
}
