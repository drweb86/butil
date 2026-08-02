using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.ImportMedia;
using BUtil.Interop.Tasks;

namespace BUtil.Tasks.ImportMedia;

public static class ImportMediaTaskPlugin
{
    public static void Register()
    {
        TaskProviderRegistry.Register<ImportMediaTaskModelOptionsV2>(
            jsonType: "ImportMedia",
            information: Resources.ImportMediaTask_Help,
            factory: (log, task, events, onMsg) => new ImportMediaRootTask(log, events, task, onMsg),
            verifier: (log, options, writeMode) =>
            {
                var destError = StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = options.DestinationFolder }, writeMode);
                if (destError != null)
                    return destError;

                var sourceError = StorageFactory.Test(log, options.From, writeMode && options.DeleteCopiedDataOnSourceMedia);
                if (sourceError != null)
                    return sourceError;

                try
                {
                    using var sourceStorage = StorageFactory.Create(log, options.From, true, 1);
                    var tooBroad = ImportMediaSourceFolderGuard.TryGetTooBroadFolderError(
                        sourceStorage.GetFolders(string.Empty));
                    if (tooBroad != null)
                        return tooBroad;
                }
                catch
                {
                    // StorageFactory.Test already succeeded; folder listing failures are non-fatal here.
                }

                var transformError = ImportMediaTransformFileName.Validate(options.TransformFileName);
                if (transformError != null)
                    return transformError;

                return null;
            });
    }
}
