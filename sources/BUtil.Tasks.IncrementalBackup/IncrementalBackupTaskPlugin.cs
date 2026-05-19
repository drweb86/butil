using BUtil.Core.Localization;
using BUtil.Core.Storages;
using BUtil.Interop.Tasks;
using BUtil.Core.TasksTree.IncrementalModel;

namespace BUtil.Tasks.IncrementalBackup;

public static class IncrementalBackupTaskPlugin
{
    public static void Register()
    {
        TaskProviderRegistry.Register<IncrementalBackupModelOptionsV2>(
            jsonType: "Incremental",
            information: Resources.IncrementalBackup_Help,
            factory: (log, task, events, onMsg) => new IncrementalBackupRootTask(log, events, task, onMsg),
            verifier: (log, options, writeMode) =>
            {
                if (string.IsNullOrWhiteSpace(options.Password))
                    return Resources.Password_Field_Validation_NotSpecified;

                if (options.Items.Count == 0)
                    return Resources.SourceItem_Validation_Empty;

                foreach (var item in options.Items)
                {
                    if (item.IsFolder)
                    {
                        if (!Directory.Exists(item.Target))
                            return string.Format(Resources.SourceItem_Validation_NotExists, item.Target);
                    }
                    else
                    {
                        return $"Please edit task and remove source item {item.Target}. Its support is dropped. You can add folders only";
                    }
                }

                return StorageFactory.Test(log, options.To, writeMode);
            });
    }
}
