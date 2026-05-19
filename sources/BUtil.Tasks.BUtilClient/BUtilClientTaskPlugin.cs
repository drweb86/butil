using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.BUtilServer.Client;
using BUtil.Interop.Tasks;

namespace BUtil.Tasks.BUtilClient;

public static class BUtilClientTaskPlugin
{
    public static void Register()
    {
        TaskProviderRegistry.Register<BUtilClientModelOptionsV2>(
            jsonType: "BUtilClient",
            information: Resources.UploadFolderTask_Help,
            factory: (log, task, events, onMsg) => new UploadFolderContentsRootTask(log, events, task, onMsg),
            verifier: (log, options, writeMode) =>
            {
                var storageError = StorageFactory.Test(log, options.To, writeMode);
                if (storageError != null)
                    return storageError;

                return StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = options.Folder }, writeMode);
            });
    }
}
