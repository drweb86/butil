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

    #region Scripts
    public bool CanLaunchScripts { get => false; }

    public string ScriptEngineName => "N/A";

    public bool LaunchScript(ILog log, string script, string _)
    {
        throw new PlatformNotSupportedException("Running shell commands in Android is not supported.");
    }

    #endregion
}
