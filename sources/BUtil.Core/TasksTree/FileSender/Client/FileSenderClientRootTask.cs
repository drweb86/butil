using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BUtil.Core.TasksTree.FileSender.Client;

internal class FileSenderClientRootTask : SequentialBuTask
{
    private readonly FileSenderClientIoc _ioc;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;
    private readonly FileSenderTransferModelOptionsV2 _options;

    public FileSenderClientRootTask(ILog log, TaskEvents backupEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, backupEvents, "File Sender Transfer", null)
    {
        var tasks = new List<BuTask>();

        _options = (FileSenderTransferModelOptionsV2)backupTask.Model;
        _ioc = new FileSenderClientIoc(log, _options.Folder, _options.Password, onGetLastMinuteMessage);

        var sourceItem = new SourceItemV2 { IsFolder = true, Target = _options.Folder };
        _getStateOfSourceItemTask = new GetStateOfSourceItemTask(Events, sourceItem, Array.Empty<string>(), _ioc.Common);
        tasks.Add(_getStateOfSourceItemTask);

        Children = tasks;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();

        var newTasks = new List<BuTask>
        {
            new FunctionBuTaskV2<bool>(_ioc.Log, Events, $"Connecting to server {_options.ServerIp}:{_options.ServerPort}",
            () =>
            {
                _ioc.Client = new System.Net.Sockets.TcpClient();
                _ioc.Client.Connect(_options.ServerIp, _options.ServerPort);
                _ioc.Stream = _ioc.Client.GetStream();
                _ioc.Writer = new BinaryWriter(_ioc.Stream, Encoding.UTF8, true);
                _ioc.FileSenderClientProtocol.WriteProtocolVersion(_ioc.Writer);
                return true;
            }),

        };
        newTasks.AddRange(_getStateOfSourceItemTask.SourceItemState!.FileStates
            .Select(fileState => new FunctionBuTaskV2<bool>(_ioc.Log, Events, $"Upload {SourceItemHelper.GetSourceItemRelativeFileName(_ioc.Model.Folder, fileState)}",
            () =>
            {
                string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(_ioc.Model.Folder, fileState);
                _ioc.Log.WriteLine(LoggingEvent.Debug, $"{relativeFileName}");

                _ioc.FileSenderClientProtocol.WriteCommandForServer(_ioc.Writer, FileTransferProtocolServerCommand.ReceiveFile);
                _ioc.FileSenderClientProtocol.WriteFileHeader(_ioc.Writer, fileState);

                var clientCommand = _ioc.FileSenderClientProtocol.ReadCommandForClient(_ioc.Stream);
                switch (clientCommand)
                {
                    case FileTransferProtocolClientCommand.Cancel:
                        _ioc.Log.WriteLine(LoggingEvent.Debug, "File transfer is skipped.");
                        break;
                    case FileTransferProtocolClientCommand.Continue:
                        _ioc.FileSenderClientProtocol.WriteFile(_ioc.Writer, fileState);
                        break;

                }
                return true;
            })));
        newTasks.Add(new FunctionBuTaskV2<bool>(_ioc.Log, Events, $"Disconnect from server",
            () =>
            {
                _ioc.FileSenderClientProtocol.WriteCommandForServer(_ioc.Writer, FileTransferProtocolServerCommand.Disconnect);
                return true;
            }));
        Events.DuringExecutionTasksAdded(Id, newTasks);
        
        Children = newTasks;
        base.Execute();
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        _ioc.Dispose();
    }
}
