using BUtil.Core.FileSystem;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace BUtil.Linux.Services;

public class SupportManager : ISupportManager
{
    public const string ApplicationName = "BUtil";
    private const string UiAppName = "butil-ui.Desktop";

    private readonly string _workDir;
    public static readonly string ConsoleBackupTool = Path.Combine(Directories.BinariesDir, "butilc");
    internal static readonly string UIApp = Path.Combine(Directories.BinariesDir, UiAppName);
    internal static readonly string ApplicationsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".local",
        "share",
        "applications");
    internal static readonly string AutostartFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".config",
        "autostart");

    public SupportManager()
    {
        _workDir = Directories.BinariesDir;
    }

    private void LaunchUiAppInternal(string? arguments = null)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "systemd-inhibit",
            WorkingDirectory = _workDir,
            Arguments = $"\"./{UiAppName}\""
                + (arguments != null ? $" {arguments}" : ""),
        });
    }

    internal static string GetScheduledTaskConsoleCommand(string taskName) =>
        $"\"{ConsoleBackupTool}\" \"Task={taskName}\"";

    internal static string GetLoginAutostartPath(string taskName) =>
        Path.Combine(AutostartFolder, $"butil-login-{Files.GetSafeFileName(taskName)}.desktop");

    internal static string CreateLoginAutostartEntry(string taskName)
    {
        var command = string.Join(" ",
            QuoteForDesktopExec(ConsoleBackupTool),
            QuoteForDesktopExec($"Task={taskName}"));

        return $"""
            [Desktop Entry]
            Type=Application
            Name={EscapeDesktopValue($"BUtil - {taskName}")}
            Exec={command}
            Path={EscapeDesktopValue(Directories.BinariesDir)}
            Terminal=false
            X-GNOME-Autostart-enabled=true
            """;
    }

    internal static string GetTaskShortcutPath(string taskName) =>
        Path.Combine(ApplicationsFolder, $"butil-task-{GetTaskHash(Files.GetTaskShortcutName(ApplicationName, taskName))}.desktop");

    internal static string CreateTaskDesktopEntry(string taskName)
    {
        var arguments = string.Join(" ",
            QuoteForDesktopExec(UIApp),
            TasksAppArguments.LaunchTask,
            QuoteForDesktopExec($"{TasksAppArguments.RunTask}={taskName}"));

        return $"""
            [Desktop Entry]
            Type=Application
            Name={EscapeDesktopValue(Files.GetTaskShortcutName(ApplicationName, taskName))}
            Exec=systemd-inhibit {arguments}
            Path={EscapeDesktopValue(Directories.BinariesDir)}
            Terminal=false
            Categories=Utility;
            StartupNotify=true
            """;
    }

    public void LaunchTasksAppOrExit()
    {
        LaunchUiAppInternal();
    }

    #region Link
    public bool CanOpenLink { get => true; }
    public bool SupportsSmileIcons => false;
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

    public bool CanLaunchScripts { get => true; }

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

    private static string GetTaskHash(string taskName)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(taskName));
        return Convert.ToHexString(hash)[..16].ToLowerInvariant();
    }

    private static string EscapeDesktopValue(string value) =>
        value
            .Replace("\\", "\\\\")
            .Replace("\r", " ")
            .Replace("\n", " ");

    private static string QuoteForDesktopExec(string value) =>
        "\"" + value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("%", "%%")
            .Replace("$", "\\$") + "\"";
}
