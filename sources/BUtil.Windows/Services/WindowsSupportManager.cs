using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Windows.Services;

public class WindowsSupportManager : ISupportManager
{
    public static readonly string UIApp =
        Path.Combine(Directories.BinariesDir, "butil-ui.Desktop.exe");
    public static readonly string ConsoleBackupTool =
        Path.Combine(Directories.BinariesDir, "butilc.exe");

    public void LaunchTasksAppOrExit()
    {
        Process.Start(UIApp);
    }

    #region Link
    public bool CanOpenLink { get => true; }
    public void OpenHomePage()
    {
        ProcessHelper.ShellExecute(ApplicationLinks.HomePage);
    }

    public void OpenLatestRelease()
    {
        ProcessHelper.ShellExecute(ApplicationLinks.LatestRelease);
    }

    public void OpenIcons()
    {
        ProcessHelper.ShellExecute(ApplicationLinks.Icons);
    }
    #endregion

    #region Scripts
    public bool CanLaunchScripts { get => true; }

    public string ScriptEngineName => "Powershell";

    public bool LaunchScript(ILog log, string script, string forbiddenForLogs)
    {
        using var tempDir = new TempFolder();
        var scriptFile = Path.Combine(tempDir.Folder, "script.ps1");
        File.WriteAllText(scriptFile, script);

        log.WriteLine(LoggingEvent.Debug, $"Executing powershell script");

        ProcessHelper.Execute("powershell.exe",
            $"& \"{scriptFile}\"",
            null,
            false,
            ProcessPriorityClass.Idle,

            out var stdOutput,
            out var stdError,
            out var returnCode);

        var isSuccess = returnCode == 0;
        if (!string.IsNullOrWhiteSpace(stdOutput))
            log.LogProcessOutput(stdOutput, isSuccess);
        if (!string.IsNullOrWhiteSpace(stdError))
            log.LogProcessOutput(stdError, isSuccess);
        if (isSuccess)
            log.WriteLine(LoggingEvent.Debug, "Executing successfull.");
        if (!isSuccess)
            log.WriteLine(LoggingEvent.Error, "Executing failed.");
        return isSuccess;
    }

    #endregion
}
