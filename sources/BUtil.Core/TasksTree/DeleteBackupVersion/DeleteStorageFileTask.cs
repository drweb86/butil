using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using BUtil.Core.TasksTree.Storage;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree;

internal class DeleteStorageFileTask : BuTask
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly string _relativeFileName;

    public DeleteStorageFileTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        string relativeFileName) :
        base(services.Log, events, string.Empty)
    {
        _services = services;
        _relativeFileName = relativeFileName;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);
        _services.Storage.Delete(_relativeFileName);
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
