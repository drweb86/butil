namespace BUtil.Core.Options;

public interface ITaskShortcutService
{
    void CreateOrUpdate(string taskName);
    void Delete(string taskName);
}
