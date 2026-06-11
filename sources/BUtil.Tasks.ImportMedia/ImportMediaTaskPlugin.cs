using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.ImportMedia;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using BUtil.Interop.Tasks;
using System.IO;

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

                if (string.IsNullOrWhiteSpace(options.TransformFileName))
                    return Resources.ImportMediaTask_Field_TransformFileName_Validation_Empty;

                try
                {
                    var str = DateTokenReplacer.ParseString(options.TransformFileName, DateTime.Now);
                    using var tempFolder = new TempFolder();
                    var fullPath = Path.Combine(tempFolder.Folder, str);
                    Directory.CreateDirectory(fullPath);
                }
                catch
                {
                    return Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
                }

                return null;
            });
    }
}
