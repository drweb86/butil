using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Tasks.IncrementalBackup.UI.Controls;

namespace BUtil.Tasks.IncrementalBackup.UI;

public static class IncrementalBackupTaskUIPlugin
{
    public static void Register()
    {
        BUtil.UI.TaskUIProviderRegistry.Register<IncrementalBackupModelOptionsV2>(
            createNewFactory: () => new EditIncrementalBackupTaskViewModel(Resources.IncrementalBackupTask_Create, true),
            editFactory: name => new EditIncrementalBackupTaskViewModel(name, false));
    }
}
