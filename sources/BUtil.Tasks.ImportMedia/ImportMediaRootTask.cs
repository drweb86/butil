using BUtil.Interop.Tasks;
using BUtil.Tasks.ImportMedia;
using BUtil.Interop.Tasks.Events;
using BUtil.Interop.Logs;
using BUtil.Core.Services;
using BUtil.Interop.Tasks.Core;
using BUtil.Tasks.Common.States;
using System;

namespace BUtil.Core.TasksTree.ImportMedia;

class ImportMediaRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;

    public ImportMediaRootTask(ILog log, TaskEvents backupEvents, TaskV2 backupTask, Action<string?> onGetLastMinuteMessage)
        : base(log, backupEvents, string.Empty)
    {
        var typedModel = (ImportMediaTaskModelOptionsV2)backupTask.Model;
        var sourceItem = new SourceItemV2(typedModel.DestinationFolder, true);

        _commonServicesIoc = new CommonServicesIoc(log, onGetLastMinuteMessage);
        var getStateOfSourceItemTask = new GetStateOfSourceItemTask(backupEvents, sourceItem, [], _commonServicesIoc);
        var importFiles = new ImportFilesTask(backupEvents, backupTask, getStateOfSourceItemTask, _commonServicesIoc);

        Children = [getStateOfSourceItemTask, importFiles];
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        _commonServicesIoc.Dispose();
    }
}
