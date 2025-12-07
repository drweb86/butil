using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using System;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

public class BUtilClientIoc
{
    public CommonServicesIoc Common { get; }
    public StorageSpecificServicesIoc StorageSpecificServices { get; }
    public BUtilClientIoc(ILog log, Action<string?> onGetLastMinuteMessage, IStorageSettingsV2 storageSettingsV2)
    {
        Common = new CommonServicesIoc(log, onGetLastMinuteMessage);
        StorageSpecificServices = new StorageSpecificServicesIoc(Common, storageSettingsV2);
    }
    public void Dispose()
    {
        StorageSpecificServices.Dispose();
        Common.Dispose();
    }
}
