using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using System.IO;
using BUtil.Core.State;
using BUtil.Core.ConfigurationFileModels.V2;
using System.Net.Sockets;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.States;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class BUtilServerSaveFileTask: BuTaskV2
{
    private readonly BUtilServerIoc _ioc;
    private readonly NetworkStream _networkStream;
    private readonly BinaryReader _reader;
    private readonly BUtilServerModelOptionsV2 _options;
    private readonly FileState _remoteFileState;

    public BUtilServerSaveFileTask(BUtilServerIoc ioc, TaskEvents events, NetworkStream networkStream, BinaryReader reader, BUtilServerModelOptionsV2 options, FileState remoteFileState, string clientPrefix) :
        base(ioc.Common.Log, events, clientPrefix + " " + string.Format(Resources.File_Saving, remoteFileState.FileName))
    {
        _ioc = ioc;
        _networkStream = networkStream;
        _reader = reader;
        _options = options;
        _remoteFileState = remoteFileState;
    }

    protected override void ExecuteInternal()
    {
        var remoteFileState = _remoteFileState;
        var fileName = new FileInfo(Path.Combine(_options.Folder, remoteFileState.FileName)).FullName;
        LogDebug($"Remote state {remoteFileState}");

        LogDebug("Ensure target directory exists.");
        Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);

        if (File.Exists(fileName))
        {
            var sourceItem = new SourceItemV2(_options.Folder, true);
            var getStateOfFileTask = new GetStateOfFileTask(Events, _ioc.Common, sourceItem, fileName);
            Events.DuringExecutionTasksAdded(Id, new BuTask[] { getStateOfFileTask });
            getStateOfFileTask.Execute();
            if (!getStateOfFileTask.IsSuccess)
            {
                throw new InvalidDataException("Can't get state of file!");
            }

            var existingFileState = getStateOfFileTask.State!;
            LogDebug($"Existing state {existingFileState}");

            if (existingFileState != null && !existingFileState.CompareTo(remoteFileState, true, true))
            {
                LogDebug("File exists, has same size, SHA-512 hash. Skipped.");
                _ioc.FileSenderServerProtocol.WriteCommandForClient(_networkStream, FileTransferProtocolClientCommand.Cancel);
                IsSkipped = true;
                return;
            }
        }

        LogDebug("Receiving the file body...");
        _ioc.FileSenderServerProtocol.WriteCommandForClient(_networkStream, FileTransferProtocolClientCommand.Continue);
        var destinationFileState = new FileState(remoteFileState);
        destinationFileState.FileName = fileName;
        _ioc.FileSenderServerProtocol.ReadFile(_reader, destinationFileState);
    }
}
