using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.TasksTree.States;

public class GetExistingVersionStateFromStorageRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly StorageSpecificServicesIoc _storageSpecificServicesIoc;
    private readonly RemoteStateLoadTask _getStateOfStorageTask;

    public GetExistingVersionStateFromStorageRootTask(ILog log, TaskEvents events, IStorageSettingsV2 storageSettings, string password, System.Action<string?> getLastMinuteMessage)
        : base(log, events, "Get existing version state from storage")
    {
        _commonServicesIoc = new CommonServicesIoc(log, getLastMinuteMessage);
        _storageSpecificServicesIoc = new StorageSpecificServicesIoc(_commonServicesIoc, storageSettings);
        _getStateOfStorageTask = new(_storageSpecificServicesIoc, events, password);
        Children = [_getStateOfStorageTask];
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        base.Execute();

        if (IsSuccess)
        {
            if (_getStateOfStorageTask.StorageState!.VersionStates.Count == 0)
            {
                _commonServicesIoc.LastMinuteMessageService.AddLastMinuteLogMessage(string.Format(Resources.RestoreFrom_Field_Validation_NoStateFiles, IncrementalBackupModelConstants.BrotliAes256V1StateFile));
                IsSuccess = false;
            }
        }

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);

        _commonServicesIoc.Dispose();
        _storageSpecificServicesIoc.Dispose();
    }

    public IncrementalBackupState? StorageState => _getStateOfStorageTask.StorageState;
}
