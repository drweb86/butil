using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.Services;
using System.Diagnostics;

namespace BUtil.Windows.Services;

public class WindowsSupportManager : ISupportManager
{
    public const string ApplicationName = "BUtil";

    public static readonly string UIApp =
        Path.Combine(Directories.BinariesDir, "butil-ui.Desktop.exe");
    public static readonly string ConsoleBackupTool =
        Path.Combine(Directories.BinariesDir, "butilc.exe");
    internal static readonly string TaskShortcutsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Programs),
        ApplicationName);

    internal static string GetTaskShortcutPath(string taskName) =>
        Path.Combine(TaskShortcutsFolder, $"{Files.GetSafeFileName(Files.GetTaskShortcutName(ApplicationName, taskName))}.lnk");

    internal static string GetTaskShortcutArguments(string taskName) =>
        $"{TasksAppArguments.LaunchTask} {QuoteArgument($"{TasksAppArguments.RunTask}={taskName}")}";

    internal static string GetTaskShortcutWorkingDirectory() => Directories.BinariesDir;

    internal static string GetTaskShortcutIconPath() => UIApp;

    public void LaunchTasksAppOrExit()
    {
        Process.Start(UIApp);
    }

    #region Link
    public bool CanOpenLink { get => true; }
    public bool SupportsSmileIcons => true;

    public void OpenLink(string url)
    {
        ProcessHelper.ShellExecute(url);
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

    #region Console command line
    public string GetConsoleCommandLineForTask(string taskName) =>
        FormatConsoleCommand(QuoteArgument($"{TasksAppArguments.RunTask}={taskName}"));

    public string GetConsoleCommandLineForEncrypt(string inputFile, string password) =>
        FormatConsoleCommand("encrypt", QuoteArgument(inputFile), QuoteArgument(password));

    public string GetConsoleCommandLineForDecrypt(string inputFile, string password) =>
        FormatConsoleCommand("decrypt", QuoteArgument(inputFile), QuoteArgument(password));

    public string GetConsoleCommandLineForCompress(string inputFile) =>
        FormatConsoleCommand("encode", QuoteArgument(inputFile));

    public string GetConsoleCommandLineForDecompress(string inputFile) =>
        FormatConsoleCommand("decode", QuoteArgument(inputFile));

    private static string FormatConsoleCommand(params string[] arguments) =>
        string.Join(" ", [QuoteArgument(ConsoleBackupTool), .. arguments]);
    #endregion

    private static string QuoteArgument(string value) =>
        "\"" + value.Replace("\"", "\\\"") + "\"";
}
