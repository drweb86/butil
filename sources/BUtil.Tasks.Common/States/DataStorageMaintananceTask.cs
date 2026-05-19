using BUtil.Interop.Tasks.Events;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Interop.Tasks.Core;
using System;
using System.Linq;

namespace BUtil.Tasks.Common.States;

internal class DataStorageMaintananceTask(
    StorageSpecificServicesIoc services,
    TaskEvents events,
    Func<IncrementalBackupState> getRemoteState) : BuTaskV2(services.CommonServices.Log, events, BUtil.Core.Localization.Resources.DataStorage_Maintenance)
{
    protected override void ExecuteInternal()
    {
        LogDebug("Delete incompleted state versions...");
        var remoteState = getRemoteState();
        var allowedFolders = remoteState
                  .VersionStates
                  .Select(x => SourceItemHelper.GetVersionFolder(x.BackupDateUtc));

        var relativeFolders = services.Storage.GetFolders(string.Empty, SourceItemHelper.GetVersionFolderMask());
        var foldersToDelete = relativeFolders.Except(allowedFolders);
        foreach (var folderToDelete in foldersToDelete)
        {
            services.Storage.DeleteFolder(folderToDelete);
        }
    }
}
