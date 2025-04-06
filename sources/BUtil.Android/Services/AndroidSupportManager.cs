using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Linux.Services;

public class AndroidSupportManager : ISupportManager
{
    public void LaunchTasksAppOrExit()
    {
        // we do nmot support it, so we exit.
        // primarily use case will be reload app on theme change
        Environment.Exit(0);
    }

    #region Link
    public bool CanOpenLink { get => false; }
    public void OpenHomePage()
    {
        throw new NotSupportedException("Opening links in Android not supported.");
    }

    public void OpenLatestRelease()
    {
        throw new NotSupportedException("Opening links in Android not supported.");
    }

    public void OpenIcons()
    {
        throw new NotSupportedException("Opening links in Android not supported.");
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
