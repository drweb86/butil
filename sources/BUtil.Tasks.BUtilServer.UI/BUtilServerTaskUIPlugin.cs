using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Tasks.BUtilServer.UI.Controls;

namespace BUtil.Tasks.BUtilServer.UI;

public static class BUtilServerTaskUIPlugin
{
    public static void Register()
    {
        BUtil.UI.TaskUIProviderRegistry.Register<BUtilServerModelOptionsV2>(
            createNewFactory: () => new EditBUtilServerTaskViewModel(Resources.FtpsServerTask_Create, true),
            editFactory: name => new EditBUtilServerTaskViewModel(name, false));
    }
}
