using BUtil.Core.Localization;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Synchronization;
using BUtil.Interop.Tasks;
using System.IO;

namespace BUtil.Tasks.Synchronization;

public static class SynchronizationTaskPlugin
{
    public static void Register()
    {
        TaskProviderRegistry.Register<SynchronizationTaskModelOptionsV2>(
            jsonType: "Synchronization",
            information: Resources.SynchronizationTask_Help,
            factory: (log, task, events, onMsg) => new SynchronizationRootTask(log, events, task, onMsg),
            verifier: (log, options, writeMode) =>
            {
                if (string.IsNullOrWhiteSpace(options.Password))
                    return Resources.Password_Field_Validation_NotSpecified;

                if (string.IsNullOrWhiteSpace(options.LocalFolder) || !Directory.Exists(options.LocalFolder))
                    return string.Format(Resources.SourceItem_Validation_NotExists, options.LocalFolder);

                return StorageFactory.Test(log, options.To, writeMode && options.SynchronizationMode == SynchronizationTaskModelMode.TwoWay);
            });
    }
}
