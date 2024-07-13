using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.TasksTree.IncrementalModel;
using System;

namespace BUtil.Core.Synchronization;

internal class SynchronizationServices : IDisposable
{
    public readonly ILog Log;
    public readonly SynchronizationLocalStateService LocalStateService;
    public readonly SynchronizationActualFilesService ActualFilesService;
    public readonly SynchronizationDecisionService DecisionService;

    public CommonServicesIoc CommonServices { get; }
    public StorageSpecificServicesIoc StorageSpecificServices { get; }

    public SynchronizationServices(ILog log, string taskName, string localFolder, string? repositorySubfolder, IStorageSettingsV2 remoteStorageSettings, bool autodetectConnectionSettings)
    {
        Log = log;
        LocalStateService = new SynchronizationLocalStateService(taskName, localFolder, repositorySubfolder);
        DecisionService = new SynchronizationDecisionService(repositorySubfolder);
        CommonServices = new CommonServicesIoc();
        ActualFilesService = new SynchronizationActualFilesService(CommonServices.HashService, localFolder);
        StorageSpecificServices = new StorageSpecificServicesIoc(log, remoteStorageSettings, CommonServices.HashService, autodetectConnectionSettings);
    }

    public void Dispose()
    {
        StorageSpecificServices.Dispose();
        CommonServices.Dispose();
    }
}
