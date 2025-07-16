using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.FileSender.Client;

internal class FileSenderClientUploadToServerFileTask : BuTaskV2
{
    private readonly FileSenderClientIoc _ioc;
    private readonly FileSenderClientModelOptionsV2 _options;
    private readonly FileState _fileState;

    public FileSenderClientUploadToServerFileTask(FileSenderClientIoc ioc, TaskEvents taskEvents, FileSenderClientModelOptionsV2 options, FileState fileState)
        : base(ioc.Common.Log, taskEvents, $"Upload {SourceItemHelper.GetSourceItemRelativeFileName(options.Folder, fileState)}")
    {
        _ioc = ioc;
        _options = options;
        _fileState = fileState;
    }

    protected override void ExecuteInternal()
    {
        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(_options.Folder, _fileState);
        LogDebug($"{relativeFileName}");

        _ioc.FileSenderClientProtocol.WriteCommandForServer(_ioc.Writer, FileTransferProtocolServerCommand.ReceiveFile);
        _ioc.FileSenderClientProtocol.WriteFileHeader(_ioc.Writer, _fileState);

        var clientCommand = _ioc.FileSenderClientProtocol.ReadCommandForClient(_ioc.Stream);
        switch (clientCommand)
        {
            case FileTransferProtocolClientCommand.Cancel:
                LogDebug("File transfer is skipped.");
                break;
            case FileTransferProtocolClientCommand.Continue:
                _ioc.FileSenderClientProtocol.WriteFile(_ioc.Writer, _fileState);
                break;

        }
    }
}
