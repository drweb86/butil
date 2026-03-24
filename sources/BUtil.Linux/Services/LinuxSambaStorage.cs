using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;
using System.Diagnostics;
using System.Text;

namespace BUtil.Linux.Services;

class LinuxSambaStorage : StorageBase<SambaStorageSettingsV2>
{
    private readonly FolderStorage _proxy;
    private readonly string _mountTarget;
    private bool _isMounted;

    internal LinuxSambaStorage(ILog log, SambaStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Url))
        {
            throw new InvalidDataException(BUtil.Core.Localization.Resources.Url_Field_Validation);
        }

        if (!string.IsNullOrWhiteSpace(this.Settings.MountPowershellScript))
        {
            if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts &&
                !PlatformSpecificExperience.Instance.SupportManager.LaunchScript(Log, this.Settings.MountPowershellScript, "***"))
                throw new InvalidOperationException($"Cannot mount");
        }

        ProcessHelper.Execute("id", "-u", null, false, System.Diagnostics.ProcessPriorityClass.Normal,
            out var stdOut, out var stdError, out var returnCode);
        if (returnCode != 0)
            throw new InvalidDataException("Cannot get user ID\n" + stdOut + "\n" + stdError);

        var userId = stdOut.Trim('\r', '\n');

        var uri = new Uri(Settings.Url);
        var host = uri.Host;
        var path = uri.AbsolutePath;
        var pathSegments = path.Split('/', '\\', StringSplitOptions.RemoveEmptyEntries);
        if (pathSegments.Length == 0)
            throw new InvalidDataException(BUtil.Core.Localization.Resources.Url_Field_Validation);
        var share = pathSegments[0];
        var folderAtShare = string.Join(
            "/",
            pathSegments
                .Skip(1)
                .ToArray());

        if (Settings.User == null)
            throw new InvalidOperationException(BUtil.Core.Localization.Resources.User_Field_Validation);

        var splittedUser = Settings.User.Split('/', '\\');
        var userDomain = splittedUser.Length == 1 ? string.Empty : splittedUser[0];
        var userWithoutDomain = splittedUser.Length == 2 ? splittedUser[1] : Settings.User;

        var destinationFolder = string.IsNullOrEmpty(folderAtShare)
            ? $"/run/user/{userId}/gvfs/smb-share:server={host},share={share}"
            : $"/run/user/{userId}/gvfs/smb-share:server={host},share={share}/{folderAtShare}";

        _mountTarget = $"smb://{host}/{share}";
        MountNetworkShare(userWithoutDomain, userDomain);

        _proxy = new FolderStorage(log, new FolderStorageSettingsV2
        {
            DestinationFolder = destinationFolder,
            SingleBackupQuotaGb = Settings.SingleBackupQuotaGb,
        });
    }

    private void MountNetworkShare(string userWithoutDomain, string userDomain)
    {
        // Cleanup stale GVFS mounts before mount attempt.
        ExecuteProcess(Log, "gio", "mount", "-u", _mountTarget, "--force");
        ExecuteProcess(Log, "killall", "gvfsd");

        var credentials = string.Join(
            Environment.NewLine,
            [
                userWithoutDomain,
                userDomain,
                Settings.Password ?? string.Empty,
                userWithoutDomain,
                userDomain,
                Settings.Password ?? string.Empty,
                userWithoutDomain,
                userDomain,
                Settings.Password ?? string.Empty,
            ]);

        if (!ExecuteProcessWithInput(Log, credentials, "gio", "mount", _mountTarget))
            throw new InvalidOperationException("Cannot mount");

        _isMounted = true;
    }

    private static bool ExecuteProcess(ILog log, string executable, params string[] arguments)
    {
        return ExecuteProcessWithInput(log, null, executable, arguments);
    }

    private static bool ExecuteProcessWithInput(ILog log, string? stdIn, string executable, params string[] arguments)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executable,
                UseShellExecute = false,
                RedirectStandardInput = stdIn != null,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            }
        };

        foreach (var argument in arguments)
            process.StartInfo.ArgumentList.Add(argument);

        var stdOutputBuilder = new StringBuilder();
        var stdErrorBuilder = new StringBuilder();
        process.OutputDataReceived += (_, a) => stdOutputBuilder.AppendLine(a.Data);
        process.ErrorDataReceived += (_, a) => stdErrorBuilder.AppendLine(a.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (stdIn != null)
        {
            process.StandardInput.WriteLine(stdIn);
            process.StandardInput.Close();
        }

        process.WaitForExit();

        var isSuccess = process.ExitCode == 0;
        var stdOutput = stdOutputBuilder.ToString();
        var stdError = stdErrorBuilder.ToString();
        if (!string.IsNullOrWhiteSpace(stdOutput))
            log.LogProcessOutput(stdOutput, isSuccess);
        if (!string.IsNullOrWhiteSpace(stdError))
            log.LogProcessOutput(stdError, isSuccess);
        return isSuccess;
    }

    public override string? Test()
    {
        return _proxy.Test();
    }

    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        return _proxy.Upload(sourceFile, relativeFileName);
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        _proxy.DeleteFolder(relativeFolderName);
    }

    public override string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        return _proxy.GetFolders(relativeFolderName, mask);
    }

    public override bool Exists(string relativeFileName)
    {
        return _proxy.Exists(relativeFileName);
    }

    public override void Delete(string relativeFileName)
    {
        _proxy.Delete(relativeFileName);
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        _proxy.Download(relativeFileName, targetFileName);
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        return _proxy.GetFiles(relativeFolderName, option);
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        return _proxy.GetModifiedTime(relativeFileName);
    }

    public override void Dispose()
    {
        _proxy.Dispose();

        if (_isMounted)
        {
            ExecuteProcess(Log, "gio", "mount", "-u", _mountTarget, "--force");
            _isMounted = false;
        }

        if (!string.IsNullOrWhiteSpace(this.Settings.UnmountPowershellScript))
        {
            if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts &&
                !PlatformSpecificExperience.Instance.SupportManager.LaunchScript(Log, this.Settings.UnmountPowershellScript, "***"))
                throw new InvalidOperationException($"Cannot unmount");
        }
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        _proxy.Move(fromRelativeFileName, toRelativeFileName);
    }
}
