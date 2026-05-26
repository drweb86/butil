using BUtil.Core.Options;

namespace BUtil.Tests.Platform;

internal sealed class StubTaskShortcutService : ITaskShortcutService
{
    public static readonly StubTaskShortcutService Instance = new();

    private StubTaskShortcutService()
    {
    }

    public void CreateOrUpdate(string taskName)
    {
    }

    public void Delete(string taskName)
    {
    }
}
