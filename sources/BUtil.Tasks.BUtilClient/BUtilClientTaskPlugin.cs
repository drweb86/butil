using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree;
using BUtil.Core.TasksTree.BUtilServer.Client;

namespace BUtil.Tasks.BUtilClient;

public static class BUtilClientTaskPlugin
{
    public static void Register()
    {
        TaskProviderRegistry.Register<BUtilClientModelOptionsV2>(
            (log, task, events, onMsg) => new UploadFolderContentsRootTask(log, events, task, onMsg),
            (log, options, writeMode) =>
            {
                var storageError = StorageFactory.Test(log, options.To, writeMode);
                if (storageError != null)
                    return storageError;

                return StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = options.Folder }, writeMode);
            });
    }
}
