using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

namespace BUtil.Core.TasksTree.Storage;

internal class DeleteStorageFileTask : BuTask
{
    private readonly StorageSpecificServicesIoc _services;
    private readonly string _relativeFileName;

    public DeleteStorageFileTask(
        StorageSpecificServicesIoc services,
        TaskEvents events,
        string relativeFileName) :
        base(services.Log, events, string.Format(Resources.File_Deleting, relativeFileName))
    {
        _services = services;
        _relativeFileName = relativeFileName;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        try
        {
            _services.Storage.Delete(_relativeFileName);
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
