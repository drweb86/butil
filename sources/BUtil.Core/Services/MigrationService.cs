using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using System;
using System.IO;

namespace BUtil.Core.Services;

public sealed class MigrationService(ILocalFileSystem fileSystem)
{
    private readonly string _folder = Path.Combine(Directories.UserDataFolder, "Migrations");

    public void RunAll()
    {
        RunOnce("MoveLogsToTempTaskFolders", MoveLogsToTempTaskFolders);
        RunOnce("MoveLogsFromTempToLocalAppDataV4", MoveLogsFromTempToLocalAppDataV4);
        RunOnce("CreateTaskShortcuts", CreateTaskShortcuts);
    }

    public void RunOnce(string migrationName, Action migration)
    {
        fileSystem.EnsureFolderCreated(_folder);

        var markerFile = Path.Combine(_folder, Files.GetSafeFileName(migrationName));
        if (fileSystem.FileExists(markerFile))
            return;

        migration();
        fileSystem.WriteAllText(markerFile, DateTimeOffset.UtcNow.ToString("O"));
    }

    private void CreateTaskShortcuts()
    {
        var taskShortcutService = PlatformSpecificExperience.Instance.GetTaskShortcutService();
        foreach (var taskName in new TaskStore(fileSystem).GetNames())
            taskShortcutService.CreateOrUpdate(taskName);
    }

    private static void MoveLogsToTempTaskFolders()
    {
        LogService.MigrateFlatLogsToTaskFolders(Directories.LegacyLogsFolder);
    }

    private static void MoveLogsFromTempToLocalAppDataV4()
    {
        LogService.MigrateLogsRoot(Directories.TempLogsFolder);
    }
}
