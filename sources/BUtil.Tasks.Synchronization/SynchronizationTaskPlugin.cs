using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree;
using BUtil.Core.TasksTree.Synchronization;
using System.IO;

namespace BUtil.Tasks.Synchronization;

public static class SynchronizationTaskPlugin
{
    public static void Register()
    {
        TaskProviderRegistry.Register<SynchronizationTaskModelOptionsV2>(
            (log, task, events, onMsg) => new SynchronizationRootTask(log, events, task, onMsg),
            (log, options, writeMode) =>
            {
                if (string.IsNullOrWhiteSpace(options.Password))
                    return Resources.Password_Field_Validation_NotSpecified;

                if (string.IsNullOrWhiteSpace(options.LocalFolder) || !Directory.Exists(options.LocalFolder))
                    return string.Format(Resources.SourceItem_Validation_NotExists, options.LocalFolder);

                return StorageFactory.Test(log, options.To, writeMode && options.SynchronizationMode == SynchronizationTaskModelMode.TwoWay);
            });
    }
}
