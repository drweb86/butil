using BUtil.Interop.Tasks;
using BUtil.Interop.Logs;
using BUtil.Interop.Tasks.Core;
using BUtil.Interop.Tasks.Events;
using System.Diagnostics.CodeAnalysis;

namespace BUtil.Interop.Tasks;

public static class TaskProviderRegistry
{
    internal sealed class ProviderEntry
    {
        internal ProviderEntry(
            string jsonType,
            string information,
            Type modelType,
            Func<ILog, TaskV2, TaskEvents, Action<string?>, BuTask> factory,
            Func<ILog, ITaskModelOptionsV2, bool, string?> verifier)
        {
            JsonType = jsonType;
            Information = information;
            ModelType = modelType;
            Factory = factory;
            Verifier = verifier;
        }

        internal string JsonType { get; }
        internal string Information { get; }
        internal Type ModelType { get; }
        internal Func<ILog, TaskV2, TaskEvents, Action<string?>, BuTask> Factory { get; }
        internal Func<ILog, ITaskModelOptionsV2, bool, string?> Verifier { get; }
    }

    private static readonly List<ProviderEntry> Entries = [];
    private static readonly object EntriesLock = new();

    public static void Register<TModel>(
        string jsonType,
        string information,
        Func<ILog, TaskV2, TaskEvents, Action<string?>, BuTask> factory,
        Func<ILog, TModel, bool, string?> verifier)
        where TModel : class, ITaskModelOptionsV2
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonType);
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(verifier);

        lock (EntriesLock)
            Entries.Add(new ProviderEntry(
                jsonType,
                information,
                typeof(TModel),
                factory,
                (log, opts, write) => verifier(log, (TModel)opts, write)));
    }

    internal static BuTask Create(ILog log, TaskV2 task, TaskEvents events, Action<string?> onGetLastMinuteMessage)
    {
        lock (EntriesLock)
        {
            var entry = Entries.FirstOrDefault(e => task.Model.GetType() == e.ModelType)
                ?? throw new ArgumentOutOfRangeException(nameof(task), $"No task provider registered for model type '{task.Model.GetType().Name}'.");
            return entry.Factory(log, task, events, onGetLastMinuteMessage);
        }
    }

    internal static bool TryVerify(ILog log, ITaskModelOptionsV2 options, bool writeMode, [NotNullWhen(false)] out string? error)
    {
        lock (EntriesLock)
        {
            var entry = Entries.FirstOrDefault(e => options.GetType() == e.ModelType)
                ?? throw new ArgumentOutOfRangeException(nameof(options), $"No task provider registered for model type '{options.GetType().Name}'.");
            error = entry.Verifier(log, options, writeMode);
            return error == null;
        }
    }

    internal static string GetInformation(Type modelType)
    {
        lock (EntriesLock)
        {
            var entry = Entries.FirstOrDefault(e => modelType == e.ModelType);
            return entry?.Information ?? string.Empty;
        }
    }

    internal static Type? FindModelType(string jsonType)
    {
        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.JsonType == jsonType)?.ModelType;
    }

    internal static string? FindJsonType(Type modelType)
    {
        lock (EntriesLock)
            return Entries.FirstOrDefault(e => e.ModelType == modelType)?.JsonType;
    }
}
