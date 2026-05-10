using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Tasks.Synchronization.UI.Controls;

namespace BUtil.Tasks.Synchronization.UI;

public static class SynchronizationTaskUIPlugin
{
    public static void Register()
    {
        BUtil.UI.TaskUIProviderRegistry.Register<SynchronizationTaskModelOptionsV2>(
            createNewFactory: () => new EditSynchronizationTaskViewModel(Resources.SynchronizationTask_Create, true),
            editFactory: name => new EditSynchronizationTaskViewModel(name, false));
    }
}
