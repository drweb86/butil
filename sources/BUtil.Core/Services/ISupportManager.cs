using BUtil.Interop.Logs;

namespace BUtil.Core.Services;

public interface ISupportManager
{
    string ScriptEngineName { get; }
    bool CanLaunchScripts { get; }
    bool LaunchScript(ILog log, string script, string forbiddenForLogs);

    void LaunchTasksAppOrExit();

    void OpenLink(string url);
    bool CanOpenLink { get; }
    bool SupportsSmileIcons { get; }

    string GetConsoleCommandLineForTask(string taskName);
    string GetConsoleCommandLineForEncrypt(string inputFile, string password);
    string GetConsoleCommandLineForDecrypt(string inputFile, string password);
    string GetConsoleCommandLineForCompress(string inputFile);
    string GetConsoleCommandLineForDecompress(string inputFile);
}
