using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BUtil.Core.ConfigurationFileModels.V2;
using System;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public partial class FolderAndPortSectionView : UserControl
{
    public FolderAndPortSectionView()
    {
        InitializeComponent();
        this.DataContext = new FolderAndPortSectionViewModel(BUtilServerModelOptionsV2.DefaultIp, BUtilServerModelOptionsV2.DefaultPort, BUtilServerModelOptionsV2.DefaultUsername, "pass", "some folder", BUtilServerModelOptionsV2.DefaultDuration);
    }

    public void BrowseCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseCommandInternal();
    }

    private async Task BrowseCommandInternal()
    {
        var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as FolderAndPortSectionViewModel ?? throw new NullReferenceException();
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.Folder);
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.ImportMediaTask_Field_OutputFolder,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        if (folders.Count == 1)
        {
            dataContext.Folder = folders[0].TryGetLocalPath() ?? folders[0].Path.ToString();
        }
    }
}
