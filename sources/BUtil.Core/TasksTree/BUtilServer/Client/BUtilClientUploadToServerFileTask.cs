using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientUploadToServerFileTask : BuTaskV2
{
    private readonly BUtilClientIoc _ioc;
    private readonly BUtilClientModelOptionsV2 _options;
    private readonly FileState _fileState;

    public BUtilClientUploadToServerFileTask(BUtilClientIoc ioc, TaskEvents taskEvents, BUtilClientModelOptionsV2 options, FileState fileState)
        : base(ioc.Common.Log, taskEvents, string.Format(Resources.File_Uploading, SourceItemHelper.GetSourceItemRelativeFileName(options.Folder, fileState)))
    {
        _ioc = ioc;
        _options = options;
        _fileState = fileState;
    }

    protected override void ExecuteInternal()
    {
        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(_options.Folder, _fileState);
        LogDebug($"{relativeFileName}");

        _ioc.Common.BUtilServerClientProtocol.WriteCommandForServer(_ioc.Writer, FileTransferProtocolServerCommand.ReceiveFile);
        _ioc.Common.BUtilServerClientProtocol.WriteFileHeader(_ioc.Writer, _fileState, _options.Folder, _options.Password);

        FileTransferProtocolClientCommand clientCommand;
        try
        {
            clientCommand = _ioc.Common.BUtilServerClientProtocol.ReadCommandForClient(_ioc.Stream);
        }
        catch (System.IO.EndOfStreamException e)
        {
            LogDebug("Passwords do not match on client and server.");
            LogError(ExceptionHelper.ToString(e));
            var fakeTaskForUi = new FunctionBuTaskV2<bool>(_ioc.Common.Log, Events, Resources.BUtilServer_Error_ConnectionAborted, () => true);
            Events.DuringExecutionTasksAdded(Id, new BuTask[] { fakeTaskForUi });
            fakeTaskForUi.Execute();
            _ioc.Common.LastMinuteMessageService.AddLastMinuteLogMessage(Resources.BUtilServer_Error_ConnectionAborted);
            throw;
        }
        switch (clientCommand)
        {
            case FileTransferProtocolClientCommand.Cancel:
                LogDebug("File transfer is skipped.");
                break;
            case FileTransferProtocolClientCommand.Continue:
                _ioc.Common.BUtilServerClientProtocol.WriteFile(_ioc.Writer, _fileState, _options.Password);
                break;

        }
    }
}
