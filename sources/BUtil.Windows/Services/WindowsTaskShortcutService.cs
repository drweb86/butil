using BUtil.Core.Options;

namespace BUtil.Windows.Services;

public sealed class WindowsTaskShortcutService : ITaskShortcutService
{
    public void CreateOrUpdate(string taskName)
    {
        try
        {
            Directory.CreateDirectory(WindowsSupportManager.TaskShortcutsFolder);
            WindowsShellLink.Save(
                WindowsSupportManager.GetTaskShortcutPath(taskName),
                WindowsSupportManager.UIApp,
                WindowsSupportManager.GetTaskShortcutArguments(taskName),
                WindowsSupportManager.GetTaskShortcutWorkingDirectory(),
                WindowsSupportManager.GetTaskShortcutIconPath());
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
            var shortcutPath = WindowsSupportManager.GetTaskShortcutPath(taskName);
            if (File.Exists(shortcutPath))
                File.Delete(shortcutPath);
        }
        catch
        {
            // Best effort cleanup.
        }
    }
}
