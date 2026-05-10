using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Tasks.ImportMedia.UI.Controls;

namespace BUtil.Tasks.ImportMedia.UI;

public static class ImportMediaTaskUIPlugin
{
    public static void Register()
    {
        BUtil.UI.TaskUIProviderRegistry.Register<ImportMediaTaskModelOptionsV2>(
            createNewFactory: () => new EditMediaTaskViewModel(string.Empty, true),
            editFactory: name => new EditMediaTaskViewModel(name, false));
    }
}
