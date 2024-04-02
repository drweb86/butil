using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.States;

public class GetExistingVersionStateFromStorageTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc = new();
    private readonly StorageSpecificServicesIoc _storageSpecificServicesIoc;
    private readonly GetStateOfStorageTask _getStateOfStorageTask;

    public GetExistingVersionStateFromStorageTask(ILog log, TaskEvents events, IStorageSettingsV2 storageSettings, string password)
        : base(log, events, "Get existing version state from storage")
    {
        _storageSpecificServicesIoc = new StorageSpecificServicesIoc(log, storageSettings, _commonServicesIoc.HashService);
        _getStateOfStorageTask = new(_storageSpecificServicesIoc, events, password);
        Children = new List<BuTask> { _getStateOfStorageTask };
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        if (IsSuccess)
        {
            if (!_getStateOfStorageTask.StorageState!.VersionStates.Any())
            {
                Events.Message(string.Format(Resources.RestoreFrom_Field_Validation_NoStateFiles, IncrementalBackupModelConstants.StorageIncrementalEncryptedCompressedStateFile));
                IsSuccess = false;
            }
        }

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);

        _commonServicesIoc.Dispose();
        _storageSpecificServicesIoc.Dispose();
    }

    public IncrementalBackupState? StorageState => _getStateOfStorageTask.StorageState;
}
