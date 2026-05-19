using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Interop.Tasks.UI;
using BUtil.Tasks.Synchronization.UI.Controls;

namespace BUtil.Tasks.Synchronization.UI;

public sealed class SynchronizationTaskUIPlugin : ITaskUIPlugin
{
    public void Register()
    {
        TaskUIProviderRegistry.Register<SynchronizationTaskModelOptionsV2>(
            createNewFactory: () => new EditSynchronizationTaskViewModel(Resources.SynchronizationTask_Create, true),
            editFactory: name => new EditSynchronizationTaskViewModel(name, false),
            createHeader: Resources.SynchronizationTask_Create,
            group: "local",
            preferredOrder: 300);
    }
}
