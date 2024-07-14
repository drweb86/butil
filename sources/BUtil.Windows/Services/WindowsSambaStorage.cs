using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Windows.Utils;
using System.IO;
using System.Reflection;
using System.Security;

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
    public override IStorageUploadResult Upload(string sourceFile, string relativeFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
        {
            var destinationFile = Path.Combine(Settings.Url, relativeFileName);
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
        var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
            ? Settings.Url
            : Path.Combine(Settings.Url, relativeFolderName);

        if (Directory.Exists(fullPathName))
            Directory.Delete(fullPathName, true);
    }

    public override string[] GetFolders(string relativeFolderName, string? mask = null)
    {
        var fullPathName = string.IsNullOrWhiteSpace(relativeFolderName)
            ? Settings.Url
            : Path.Combine(Settings.Url, relativeFolderName);

        return Directory
            .GetDirectories(fullPathName, mask ?? "*.*")
            .Select(x => x.Substring(fullPathName.Length))
            .Select(x => x.Trim(new[] { '\\', '/' }))
            .ToArray();
    }

    private void Mount()
    {
        Log.WriteLine(LoggingEvent.Debug, $"Mount");

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
    }

    public override string? Test(bool writeMode)
    {
        if (!Directory.Exists(Settings.Url))
            return string.Format(Resources.DirectoryStorage_Field_Directory_Validation_NotFound, Settings.Url); ;

        if (writeMode)
        {
            var folder = Guid.NewGuid().ToString();
            var file = Path.Combine("BUtil check " + folder, Guid.NewGuid().ToString());
            var uploaded = Upload(Assembly.GetExecutingAssembly().Location, file);
            if (uploaded == null)
                throw new Exception("Failed to upload!");
            DeleteFolder(folder);
        }

        return null;
    }

    public override bool Exists(string relativeFileName)
    {
        var fullPathName = Path.Combine(Settings.Url, relativeFileName);

        return File.Exists(fullPathName);
    }

    public override void Delete(string relativeFileName)
    {
        var fullPathName = Path.Combine(Settings.Url, relativeFileName);

        if (File.Exists(fullPathName))
            File.Delete(fullPathName);
    }

    public override void Download(string relativeFileName, string targetFileName)
    {
        lock (_uploadLock) // because we're limited by upload speed and Samba has limit of 6 parallel uploads usually
        {
            var file = Path.Combine(Settings.Url, relativeFileName);
            Copy(file, targetFileName);
        }
    }

    public static void Copy(string inputFile, string outputFilePath)
    {
        int bufferSize = 16 * 1024 * 1024;

        using var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize);
        using var outputFileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
        outputFileStream.SetLength(inputFileStream.Length);
        int bytesRead = -1;
        byte[] bytes = new byte[bufferSize];

        while ((bytesRead = inputFileStream.Read(bytes, 0, bufferSize)) > 0)
        {
            outputFileStream.Write(bytes, 0, bytesRead);
        }
    }

    public override string[] GetFiles(string? relativeFolderName = null, SearchOption option = SearchOption.TopDirectoryOnly)
    {
        // add security.
        var actualFolder = relativeFolderName == null ?
            Settings.Url :
            Path.Combine(Settings.Url, relativeFolderName);

        if (relativeFolderName != null)
        {
            if (relativeFolderName.Contains(".."))
                throw new SecurityException(nameof(relativeFolderName));
        }

        return Directory
            .GetFiles(actualFolder, "*.*", option)
            .Select(x => x.Substring(actualFolder.Length))
            .Select(x => x.Trim(new[] { '\\', '/' }))
            .ToArray();
    }

    public override DateTime GetModifiedTime(string relativeFileName)
    {
        var actualFile = Path.Combine(Settings.Url, relativeFileName);
        return File.GetLastWriteTime(actualFile);
    }

    public override void Dispose()
    {
        Unmount();
    }

    public override void Move(string fromRelativeFileName, string toRelativeFileName)
    {
        var from = Path.Combine(Settings.Url, fromRelativeFileName);
        var to = Path.Combine(Settings.Url, toRelativeFileName);

        var destinationDirectory = Path.GetDirectoryName(to) ?? string.Empty;
        if (!Directory.Exists(destinationDirectory))
            Directory.CreateDirectory(destinationDirectory);

        File.Move(from, to);
    }
}
