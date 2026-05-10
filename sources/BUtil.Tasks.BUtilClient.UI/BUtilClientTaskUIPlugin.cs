using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Tasks.BUtilClient.UI.Controls;

namespace BUtil.Tasks.BUtilClient.UI;

public static class BUtilClientTaskUIPlugin
{
    public static void Register()
    {
        BUtil.UI.TaskUIProviderRegistry.Register<BUtilClientModelOptionsV2>(
            createNewFactory: () => new EditBUtilServerClientTaskViewModel(Resources.UploadFolderContentsTask_Create, true),
            editFactory: name => new EditBUtilServerClientTaskViewModel(name, false));
    }
}
