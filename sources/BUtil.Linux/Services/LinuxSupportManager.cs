using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services;

public class LinuxSupportManager : ISupportManager
{
    private readonly string _workDir;
    private readonly string _uiApp;

    public LinuxSupportManager()
    {
        _workDir = Directories.BinariesDir;
        _uiApp = "butil-ui.Desktop";
    }

    private void LaunchUiAppInternal(string? arguments = null)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "systemd-inhibit",
            WorkingDirectory = _workDir,
            Arguments = $"\"./{_uiApp}\""
                + (arguments != null ? $" {arguments}" : ""),
        });
    }

    public void LaunchTasksAppOrExit()
    {
        LaunchUiAppInternal();
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

    public string ScriptEngineName => "Bash";

    public bool LaunchScript(ILog log, string script, string _)
    {
        script = script.Replace("\r", string.Empty);

        using var tempDir = new TempFolder();
        var scriptFile = Path.Combine(tempDir.Folder, "script.sh");
        File.WriteAllText(scriptFile, "#!/bin/bash" + Environment.NewLine + script);

        log.WriteLine(LoggingEvent.Debug, $"Set permissions to script");

        ProcessHelper.Execute("chmod", $"-v +x \"{scriptFile}\"",
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
        if (!isSuccess)
            log.WriteLine(LoggingEvent.Error, "Executing permissions failed.");

        log.WriteLine(LoggingEvent.Debug, $"Executing script");

        ProcessHelper.Execute("bash",
            $"-c \"{scriptFile}\"",
            null,
            false,
            ProcessPriorityClass.Idle,

            out stdOutput,
            out stdError,
            out returnCode);

        isSuccess = returnCode == 0;
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
}
