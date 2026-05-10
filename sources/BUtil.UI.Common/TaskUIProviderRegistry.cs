using BUtil.UI.Controls;
using System;
using System.Collections.Generic;

namespace BUtil.UI;

public static class TaskUIProviderRegistry
{
    private sealed record TaskUIEntry(
        Func<ViewModelBase> CreateNewFactory,
        Func<string, ViewModelBase> EditFactory);

    private static readonly Dictionary<Type, TaskUIEntry> _entries = [];
    private static readonly object _lock = new();

    public static void Register<TModel>(
        Func<ViewModelBase> createNewFactory,
        Func<string, ViewModelBase> editFactory)
        where TModel : class
    {
        lock (_lock)
            _entries[typeof(TModel)] = new TaskUIEntry(createNewFactory, editFactory);
    }

    public static ViewModelBase? CreateNew(Type modelType)
    {
        lock (_lock)
            return _entries.TryGetValue(modelType, out var e) ? e.CreateNewFactory() : null;
    }

    public static ViewModelBase? CreateEdit(Type modelType, string taskName)
    {
        lock (_lock)
            return _entries.TryGetValue(modelType, out var e) ? e.EditFactory(taskName) : null;
    }
}
