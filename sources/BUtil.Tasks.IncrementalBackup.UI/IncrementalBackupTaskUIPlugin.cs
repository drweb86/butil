using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Interop.Tasks.UI;
using BUtil.Tasks.IncrementalBackup.UI.Controls;

namespace BUtil.Tasks.IncrementalBackup.UI;

public sealed class IncrementalBackupTaskUIPlugin : ITaskUIPlugin
{
    public void Register()
    {
        TaskUIProviderRegistry.Register<IncrementalBackupModelOptionsV2>(
            createNewFactory: () => new EditIncrementalBackupTaskViewModel(Resources.IncrementalBackupTask_Create, true),
            editFactory: name => new EditIncrementalBackupTaskViewModel(name, false),
            createHeader: Resources.IncrementalBackupTask_Create,
            group: "local",
            preferredOrder: 100);
    }
}
