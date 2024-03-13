using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BUtil.Core.ConfigurationFileModels.V2;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public partial class WhereTaskView : UserControl
{
    public WhereTaskView()
    {
        InitializeComponent();

        var viewModel = new Controls.WhereTaskViewModel(new FolderStorageSettingsV2(), BUtil.Core.Localization.Resources.LeftMenu_Where, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
        this.DataContext = viewModel;
        this.MtpList.DropDownOpened += UpdateListMtpDevices;
    }

    public void BrowseCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseCommandInternal();
    }

    private async Task BrowseCommandInternal()
    {
        var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as WhereTaskViewModel ?? throw new NullReferenceException();
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.FolderFolder);
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.Field_Folder,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        var folder = folders.FirstOrDefault();
        if (folder != null)
        {
            dataContext.FolderFolder = folder.TryGetLocalPath() ?? folder.Path.ToString();
        }
    }

    private void UpdateListMtpDevices(object? sender, System.EventArgs e)
    {
        var dataContext = DataContext as WhereTaskViewModel ?? throw new NullReferenceException();
        dataContext.UpdateListMtpDevices();
    }
}
