﻿using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using System;

namespace BUtil.Core.Synchronization;

internal class SynchronizationServices : IDisposable
{
    public readonly SynchronizationLocalStateService LocalStateService;
    public readonly SynchronizationActualFilesService ActualFilesService;
    public readonly SynchronizationDecisionService DecisionService;

    public CommonServicesIoc CommonServices { get; }
    public StorageSpecificServicesIoc StorageSpecificServices { get; }

    public SynchronizationServices(ILog log, string taskName, string localFolder, string? repositorySubfolder, IStorageSettingsV2 remoteStorageSettings, bool autodetectConnectionSettings, Action<string?> onGetLastMinuteMessage)
    {
        LocalStateService = new SynchronizationLocalStateService(taskName, localFolder, repositorySubfolder);
        DecisionService = new SynchronizationDecisionService(repositorySubfolder);
        CommonServices = new CommonServicesIoc(log, onGetLastMinuteMessage);
        ActualFilesService = new SynchronizationActualFilesService(CommonServices.CachedHashService, localFolder);
        StorageSpecificServices = new StorageSpecificServicesIoc(CommonServices, remoteStorageSettings, autodetectConnectionSettings);
    }

    public void Dispose()
    {
        StorageSpecificServices.Dispose();
        CommonServices.Dispose();
    }
}
