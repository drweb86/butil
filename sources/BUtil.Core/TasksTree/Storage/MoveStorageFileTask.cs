using BUtil.Core.Events;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

namespace BUtil.Core.TasksTree.Storage;

internal class MoveStorageFileTask : BuTask
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly string _fromRelativeFileName;
    private readonly string _toRelativeFileName;

    public MoveStorageFileTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        string fromRelativeFileName,
        string toRelativeFileName) :
        base(services.Log, events, $"Move storage file \"{fromRelativeFileName}\" to \"{toRelativeFileName}\"")
    {
        _services = services;
        _fromRelativeFileName = fromRelativeFileName;
        _toRelativeFileName = toRelativeFileName;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        try
        {
            _services.Storage.Move(_fromRelativeFileName, _toRelativeFileName);
            IsSuccess = true;
        }
        catch (Exception e)
        {
            LogError(e.Message);
            IsSuccess = false;
        }

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
