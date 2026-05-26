using BUtil.Core.Options;

namespace BUtil.Linux.Services;

public sealed class LinuxTaskShortcutService : ITaskShortcutService
{
    public void CreateOrUpdate(string taskName)
    {
        try
        {
            Directory.CreateDirectory(LinuxSupportManager.ApplicationsFolder);

            var shortcutPath = LinuxSupportManager.GetTaskShortcutPath(taskName);
            File.WriteAllText(shortcutPath, LinuxSupportManager.CreateTaskDesktopEntry(taskName));
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
            var shortcutPath = LinuxSupportManager.GetTaskShortcutPath(taskName);
            if (File.Exists(shortcutPath))
                File.Delete(shortcutPath);
        }
        catch
        {
            // Best effort cleanup.
        }
    }
}
