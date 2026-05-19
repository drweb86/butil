namespace BUtil.Interop.Tasks.UI;

public static class TaskUIProviderRegistry
{
    private sealed record TaskUIEntry(
        Func<object> CreateNewFactory,
        Func<string, object> EditFactory,
        string CreateHeader,
        string Group,
        int PreferredOrder);

    private static readonly Dictionary<Type, TaskUIEntry> _entries = [];
    private static readonly object _lock = new();

    public static void Register<TModel>(
        Func<object> createNewFactory,
        Func<string, object> editFactory,
        string createHeader,
        string group = "",
        int preferredOrder = 0)
        where TModel : class
    {
        ArgumentNullException.ThrowIfNull(createNewFactory);
        ArgumentNullException.ThrowIfNull(editFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(createHeader);

        lock (_lock)
        {
            _entries[typeof(TModel)] = new TaskUIEntry(
                createNewFactory,
                editFactory,
                createHeader,
                group ?? string.Empty,
                preferredOrder);
        }
    }

    internal static object? CreateNew(Type modelType)
    {
        lock (_lock)
            return _entries.TryGetValue(modelType, out var e) ? e.CreateNewFactory() : null;
    }

    internal static object? CreateEdit(Type modelType, string taskName)
    {
        lock (_lock)
            return _entries.TryGetValue(modelType, out var e) ? e.EditFactory(taskName) : null;
    }

    internal static string GetCreateHeader(Type modelType)
    {
        lock (_lock)
            return _entries.TryGetValue(modelType, out var e) ? e.CreateHeader : string.Empty;
    }

    internal static IReadOnlyList<TaskUICreateMenuRegistration> GetCreateMenuRegistrations()
    {
        lock (_lock)
        {
            return _entries
                .Select(e => new TaskUICreateMenuRegistration(
                    e.Key,
                    e.Value.CreateHeader,
                    e.Value.Group,
                    e.Value.PreferredOrder))
                .OrderBy(e => e.Group, StringComparer.CurrentCultureIgnoreCase)
                .ThenBy(e => e.PreferredOrder)
                .ThenBy(e => e.Header, StringComparer.CurrentCultureIgnoreCase)
                .ToArray();
        }
    }
}
