using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.States;
using System;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel;

class ImportSingleFileTask : BuTaskV2
{
    private readonly string _transformFileName;
    private readonly IStorage _fromStorage;
    private readonly IStorage _toStorage;
    private readonly SourceItemState _state;
    private readonly CommonServicesIoc _commonServicesIoc;

    public string File { get; private set; }

    public ImportSingleFileTask(
        TaskEvents backupEvents,
        string fromFile,
        IStorage fromStorage,
        IStorage toStorage,
        string transformFileName,
        SourceItemState state,
        CommonServicesIoc commonServicesIoc)
        : base(commonServicesIoc.Log, backupEvents, string.Empty)
    {
        File = fromFile;
        _state = state;
        _commonServicesIoc = commonServicesIoc;
        _fromStorage = fromStorage;
        _toStorage = toStorage;
        _transformFileName = transformFileName;

        Title = string.Format(BUtil.Core.Localization.Resources.ImportMediaTask_File, Path.GetFileNameWithoutExtension(fromFile));
    }

    protected override void ExecuteInternal()
    {
        var lastWriteTime = _fromStorage.GetModifiedTime(this.File);
        var destinationFileName = GetDestinationFileName(lastWriteTime);
        var actualFileName = GetActualDestinationFileName(destinationFileName);
        var destFolder = Path.GetDirectoryName(actualFileName);

        using var temp = new TempFolder();
        var exchangeFile = Path.Combine(temp.Folder, Path.GetFileName(this.File));
        _fromStorage.Download(this.File, exchangeFile);
        System.IO.File.SetCreationTime(exchangeFile, lastWriteTime);
        System.IO.File.SetLastWriteTime(exchangeFile, lastWriteTime);

        var fileInfo = new FileInfo(exchangeFile);
        var state = new FileState(exchangeFile, fileInfo.LastWriteTimeUtc, fileInfo.Length, this._commonServicesIoc.HashService.GetSha512(exchangeFile, true));

        if (_state.FileStates.Any(x => x.CompareTo(state, true)))
        {
            Log.WriteLine(LoggingEvent.Debug, $"File {File} is already exists in destination folder. Skipping.");
            IsSkipped = true;
            return;
        }

        var uploadedFileName = _toStorage.Upload(exchangeFile, actualFileName) ?? throw new IOException($"Failed to upload file {exchangeFile}");
        System.IO.File.SetCreationTime(uploadedFileName.StorageFileName, lastWriteTime);
        System.IO.File.SetLastWriteTime(uploadedFileName.StorageFileName, lastWriteTime);
    }

    private string GetActualDestinationFileName(string destFile)
    {
        var actualDestFile = destFile;
        var id = 0;
        while (_toStorage.Exists(actualDestFile))
        {
            id++;
            var fileName = $"{Path.GetFileNameWithoutExtension(destFile)}_{id}{Path.GetExtension(destFile)}";
            var dirName = Path.GetDirectoryName(destFile);
            actualDestFile = !string.IsNullOrWhiteSpace(dirName) ? Path.Combine(dirName, fileName) : fileName;
        }

        return actualDestFile;
    }

    private string GetDestinationFileName(DateTime lastWriteTime)
    {
        var relativePath = DateTokenReplacer.ParseString(_transformFileName, lastWriteTime);
        var relativeDir = Path.GetDirectoryName(relativePath) ?? string.Empty;
        foreach (var ch in Path.GetInvalidPathChars())
        {
            relativeDir = relativeDir.Replace(ch, '_');
        }

        var relativeFileName = Path.GetFileName(relativePath);
        foreach (var ch in Path.GetInvalidFileNameChars())
        {
            relativeFileName = relativeFileName.Replace(ch, '_');
        }

        var fileName = $"{relativeFileName}{Path.GetExtension(this.File)}";
        var destFile = string.IsNullOrWhiteSpace(relativeDir) ? fileName : Path.Combine(relativeDir, fileName);

        return destFile;
    }
}
