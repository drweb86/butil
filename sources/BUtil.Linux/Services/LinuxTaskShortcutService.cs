using BUtil.Core.Options;

namespace BUtil.Linux.Services;

public sealed class LinuxTaskShortcutService : ITaskShortcutService
{
    public void CreateOrUpdate(string taskName)
    {
        try
        {
            Directory.CreateDirectory(SupportManager.ApplicationsFolder);

            var shortcutPath = SupportManager.GetTaskShortcutPath(taskName);
            File.WriteAllText(shortcutPath, SupportManager.CreateTaskDesktopEntry(taskName));
        }
        catch
        {
            // Shortcuts should not prevent task changes from being saved.
        }
    }

    public void Delete(string taskName)
    {
        try
        {
            var shortcutPath = SupportManager.GetTaskShortcutPath(taskName);
            if (File.Exists(shortcutPath))
                File.Delete(shortcutPath);
        }
        catch
        {
            // Best effort cleanup.
        }
    }
}
