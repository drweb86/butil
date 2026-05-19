using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Interop.Tasks.UI;
using BUtil.Tasks.ImportMedia.UI.Controls;

namespace BUtil.Tasks.ImportMedia.UI;

public sealed class ImportMediaTaskUIPlugin : ITaskUIPlugin
{
    public void Register()
    {
        TaskUIProviderRegistry.Register<ImportMediaTaskModelOptionsV2>(
            createNewFactory: () => new EditMediaTaskViewModel(string.Empty, true),
            editFactory: name => new EditMediaTaskViewModel(name, false),
            createHeader: Resources.ImportMediaTask_Create,
            group: "local",
            preferredOrder: 200);
    }
}
