using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Interop.Tasks.UI;
using BUtil.Tasks.BUtilServer.UI.Controls;

namespace BUtil.Tasks.BUtilServer.UI;

public sealed class BUtilServerTaskUIPlugin : ITaskUIPlugin
{
    public void Register()
    {
        TaskUIProviderRegistry.Register<BUtilServerModelOptionsV2>(
            createNewFactory: () => new EditBUtilServerTaskViewModel(Resources.FtpsServerTask_Create, true),
            editFactory: name => new EditBUtilServerTaskViewModel(name, false),
            createHeader: Resources.FtpsServerTask_Create,
            group: "transfer",
            preferredOrder: 100);
    }
}
