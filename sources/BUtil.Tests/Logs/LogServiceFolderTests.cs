using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Localization;
using BUtil.Core;
using System.Globalization;
using System.IO;

namespace BUtil.Tests.Logs;

[TestClass]
public class LogServiceFolderTests
{
    private const string DateMask = "yyyy-MM-dd HH-mm-ss";

    [TestMethod]
    public void GetFileName_PlacesLogInTaskSubfolder()
    {
        var taskName = "folder-test-" + Guid.NewGuid().ToString("N");
        var dateTime = new DateTime(2024, 5, 6, 7, 8, 9);

        var path = LogService.GetFileName(taskName, dateTime, isSuccess: true);

        Assert.AreEqual(LogService.GetTaskLogsFolder(taskName), Path.GetDirectoryName(path));
        Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(path)));
        Assert.IsTrue(path.StartsWith(Directories.LogsFolder, StringComparison.OrdinalIgnoreCase));

        try
        {
            if (File.Exists(path))
                File.Delete(path);
            var folder = LogService.GetTaskLogsFolder(taskName);
            if (Directory.Exists(folder) && !Directory.EnumerateFileSystemEntries(folder).Any())
                Directory.Delete(folder);
        }
        catch
        {
            // cleanup best-effort
        }
    }

    [TestMethod]
    public void MigrateFlatLogsToTaskFolders_MovesFilesIntoTaskSubfolders()
    {
        var flatFolder = Path.Combine(Path.GetTempPath(), "butil-log-migrate-" + Guid.NewGuid().ToString("N"));
        var taskName = "migrate-task";
        var dateTime = new DateTime(2023, 1, 2, 3, 4, 5);
        Directory.CreateDirectory(flatFolder);

        var flatFileName = BuildLogFileName(dateTime, taskName, Resources.LogFile_Marker_Successful);
        var flatFile = Path.Combine(flatFolder, flatFileName);
        File.WriteAllText(flatFile, "log");

        try
        {
            LogService.MigrateFlatLogsToTaskFolders(flatFolder);

            Assert.IsFalse(File.Exists(flatFile));
            var expected = LogService.GetFileName(taskName, dateTime, isSuccess: true);
            Assert.IsTrue(File.Exists(expected));
        }
        finally
        {
            if (Directory.Exists(flatFolder))
                Directory.Delete(flatFolder, recursive: true);

            var taskFolder = LogService.GetTaskLogsFolder(taskName);
            if (Directory.Exists(taskFolder))
                Directory.Delete(taskFolder, recursive: true);
        }
    }

    [TestMethod]
    public void GetRecentLogs_FindsLogsInTaskSubfolders()
    {
        var taskName = "recent-logs-" + Guid.NewGuid().ToString("N");
        var dateTime = new DateTime(2022, 6, 7, 8, 9, 10);
        var path = LogService.GetFileName(taskName, dateTime, isSuccess: false);
        File.WriteAllText(path, "log");

        try
        {
            var recent = new LogService().GetRecentLogs().FirstOrDefault(x => x.TaskName.Cmp(taskName));

            Assert.IsNotNull(recent);
            Assert.AreEqual(path, recent.File);
            Assert.IsFalse(recent.IsSuccess!.Value);
        }
        finally
        {
            LogService.DeleteLogs(taskName);
        }
    }

    [TestMethod]
    public void MoveLogs_RenamesTaskFolderAndUpdatesFilenames()
    {
        var oldName = "move-old-" + Guid.NewGuid().ToString("N");
        var newName = "move-new-" + Guid.NewGuid().ToString("N");
        var dateTime = new DateTime(2021, 3, 4, 5, 6, 7);
        var path = LogService.GetFileName(oldName, dateTime, isSuccess: true);
        File.WriteAllText(path, "log");

        try
        {
            LogService.MoveLogs(oldName, newName);

            Assert.IsFalse(Directory.Exists(LogService.GetTaskLogsFolder(oldName)));
            var newPath = LogService.GetFileName(newName, dateTime, isSuccess: true);
            Assert.IsTrue(File.Exists(newPath));
            Assert.IsFalse(File.Exists(path));
        }
        finally
        {
            LogService.DeleteLogs(newName);
        }
    }

    [TestMethod]
    public void DeleteLogs_RemovesTaskFolder()
    {
        var taskName = "delete-logs-" + Guid.NewGuid().ToString("N");
        var path = LogService.GetFileName(taskName, DateTime.Now, isSuccess: null);
        File.WriteAllText(path, "log");

        LogService.DeleteLogs(taskName);

        Assert.IsFalse(Directory.Exists(LogService.GetTaskLogsFolder(taskName)));
        Assert.IsFalse(File.Exists(path));
    }

    private static string BuildLogFileName(DateTime dateTime, string taskName, string marker) =>
        $"{dateTime.ToString(DateMask, CultureInfo.CurrentUICulture)} {taskName} ({marker}).txt";
}
