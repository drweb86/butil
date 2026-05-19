using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Interop.Tasks.UI;
using BUtil.Tasks.BUtilClient.UI.Controls;

namespace BUtil.Tasks.BUtilClient.UI;

public sealed class BUtilClientTaskUIPlugin : ITaskUIPlugin
{
    public void Register()
    {
        TaskUIProviderRegistry.Register<BUtilClientModelOptionsV2>(
            createNewFactory: () => new EditBUtilServerClientTaskViewModel(Resources.UploadFolderContentsTask_Create, true),
            editFactory: name => new EditBUtilServerClientTaskViewModel(name, false),
            createHeader: Resources.UploadFolderContentsTask_Create,
            group: "transfer",
            preferredOrder: 200);
    }
}
