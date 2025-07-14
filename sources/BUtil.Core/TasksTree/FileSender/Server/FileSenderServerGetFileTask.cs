using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using System.IO;
using System.Linq;
using BUtil.Core.State;
using BUtil.Core.ConfigurationFileModels.V2;

namespace BUtil.Core.TasksTree.FileSender.Server;

internal class FileSenderServerGetFileTask: BuTaskV2
{
    private readonly FileSenderServerIoc _ioc;
    private readonly FileState _fileState;
    private readonly SourceItemState _sourceItemState;
    private readonly FileSenderServerModelOptionsV2 _options;

    public FileSenderServerGetFileTask(FileSenderServerIoc ioc, TaskEvents events, FileState fileState, SourceItemState sourceItemState, FileSenderServerModelOptionsV2 options) :
        base(ioc.Common.Log, events, $"Upload to server {SourceItemHelper.GetSourceItemRelativeFileName(options.Folder, fileState)}")
    {
        _ioc = ioc;
        _fileState = fileState;
        _sourceItemState = sourceItemState;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        LogDebug("Ensure target directory exists.");
        Directory.CreateDirectory(Path.GetDirectoryName(_fileState.FileName)!);

        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(_options.Folder, _fileState);

        var existingFileState = _sourceItemState.FileStates.SingleOrDefault(s => s.FileName == _fileState.FileName);
        LogDebug($"Remote state {_fileState}");
        LogDebug($"Existing state {existingFileState}");
        if (existingFileState != null && !existingFileState.CompareTo(_fileState, true))
        {
            LogDebug("File exists, has same size, SHA-512 hash. Skipped.");
            _ioc.FileSenderServerProtocol.WriteCommandForClient(_ioc.Stream, FileTransferProtocolClientCommand.Cancel);
            IsSkipped = true;
        }
        else
        {
            LogDebug("Receiving the file body...");
            _ioc.FileSenderServerProtocol.WriteCommandForClient(_ioc.Stream, FileTransferProtocolClientCommand.Continue);
            _ioc.FileSenderServerProtocol.ReadFile(_ioc.Reader, _fileState);
        }
    }
}
