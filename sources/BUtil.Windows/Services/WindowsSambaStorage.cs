using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Windows.Utils;

namespace BUtil.Windows.Services;

class WindowsSambaStorage : StorageBase<SambaStorageSettingsV2>
{
    internal WindowsSambaStorage(ILog log, SambaStorageSettingsV2 settings)
        : base(log, settings)
    {
        if (string.IsNullOrWhiteSpace(Settings.Url))
        {
            throw new InvalidDataException(BUtil.Core.Localization.Resources.Url_Field_Validation);
        }

        Mount();
    }

    private readonly object _uploadLock = new();
    private bool _isDisposed;

    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
        {
            var destinationFile = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFileName, allowEmpty: false, nameof(relativeFileName));
            Log.WriteLine(LoggingEvent.Debug, $"Copying \"{sourceFile}\" to \"{destinationFile}\"");

            var destinationDirectory = Path.GetDirectoryName(destinationFile) ?? string.Empty;
            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            Copy(sourceFile, destinationFile);

            return new IStorageUploadResult
            {
                StorageFileName = destinationFile,
                StorageFileNameSize = new FileInfo(destinationFile).Length,
            };
        }
    }

    public override void DeleteFolder(string relativeFolderName)
    {
        var fullPathName = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFolderName, allowEmpty: true, nameof(relativeFolderName));

        if (Directory.Exists(fullPathName))
            Directory.Delete(fullPathName, true);
    }

    public override string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        var fullPathName = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFolderName, allowEmpty: true, nameof(relativeFolderName));

        return Directory
            .GetDirectories(fullPathName, mask ?? "*")
            .Select(x => x[fullPathName.Length..])
            .Select(x => x.Trim(['\\', '/']))
            .ToArray();
    }

    private void Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Mount");

        if (!string.IsNullOrWhiteSpace(this.Settings.MountPowershellScript))
        {
            if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts &&
                !PlatformSpecificExperience.Instance.SupportManager.LaunchScript(Log, this.Settings.MountPowershellScript, "***"))
                throw new InvalidOperationException($"Cannot mount");
        }

        if (string.IsNullOrWhiteSpace(Settings.User))
            return;

        var command = @$"net use ""{Settings.Url}"" ""/user:{Settings.User}"" ""{Settings.Password}""";

        if (!WindowsCmdProcessHelper.Execute(Log, command))
            throw new InvalidOperationException($"Cannot mount");
    }

    private void Unmount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Unmount");

        if (string.IsNullOrWhiteSpace(Settings.User))
            return;

        var command = @$"net use ""{Settings.Url}"" /delete /y";
        if (!WindowsCmdProcessHelper.Execute(Log, command))
            throw new InvalidOperationException($"Cannot unmount");

        if (!string.IsNullOrWhiteSpace(this.Settings.UnmountPowershellScript))
        {
            if (PlatformSpecificExperience.Instance.SupportManager.CanLaunchScripts &&
                !PlatformSpecificExperience.Instance.SupportManager.LaunchScript(Log, this.Settings.UnmountPowershellScript, "***"))
                throw new InvalidOperationException($"Cannot unmount");
        }
    }

    public override string? Test()
    {
        if (!Directory.Exists(Settings.Url))
            return string.Format(Resources.DirectoryStorage_Field_Directory_Validation_NotFound, Settings.Url); ;

        return null;
    }

    public override bool Exists(string relativeFileName)
    {
        var fullPathName = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFileName, allowEmpty: false, nameof(relativeFileName));

        return File.Exists(fullPathName);
    }

    public override void Delete(string relativeFileName)
    {
        var fullPathName = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFileName, allowEmpty: false, nameof(relativeFileName));

        if (File.Exists(fullPathName))
            File.Delete(fullPathName);
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
        {
            var file = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFileName, allowEmpty: false, nameof(relativeFileName));
            Copy(file, targetFileName);
        }
    }

    public static void Copy(string inputFile, string outputFilePath)
    {
        int bufferSize = 16 * 1024 * 1024;
        var temporaryFilePath = outputFilePath + ".tmp." + Guid.NewGuid().ToString("N");

        try
        {
            using var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize);
            using var outputFileStream = new FileStream(temporaryFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize);
            outputFileStream.SetLength(inputFileStream.Length);
            int bytesRead = -1;
            byte[] bytes = new byte[bufferSize];

            while ((bytesRead = inputFileStream.Read(bytes, 0, bufferSize)) > 0)
            {
                outputFileStream.Write(bytes, 0, bytesRead);
            }

            outputFileStream.Flush(true);

            File.Move(temporaryFilePath, outputFilePath, true);
        }
        catch (IOException)
        {
            try
            {
                if (File.Exists(temporaryFilePath))
                    File.Delete(temporaryFilePath);
            }
            catch
            {
                // ignored: this copy helper has no logger and should preserve original exception
            }
            throw;
        }
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        var actualFolder = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFolderName, allowEmpty: true, nameof(relativeFolderName));

        return Directory
            .GetFiles(actualFolder, "*", option)
            .Select(x => x[actualFolder.Length..])
            .Select(x => x.Trim(['\\', '/']))
            .ToArray();
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var actualFile = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, relativeFileName, allowEmpty: false, nameof(relativeFileName));
        return File.GetLastWriteTime(actualFile);
    }

    public override void Dispose()
    {
        if (_isDisposed)
            return;

        Unmount();
        _isDisposed = true;
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        var from = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, fromRelativeFileName, allowEmpty: false, nameof(fromRelativeFileName));
        var to = StoragePathSecurity.ResolveRelativePathInsideRoot(Settings.Url, toRelativeFileName, allowEmpty: false, nameof(toRelativeFileName));

        var destinationDirectory = Path.GetDirectoryName(to) ?? string.Empty;
        if (!Directory.Exists(destinationDirectory))
            Directory.CreateDirectory(destinationDirectory);

        File.Move(from, to);
    }
}
