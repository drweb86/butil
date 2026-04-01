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
    }

    public void BrowseCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseCommandInternal();
    }

    public void KeyFileBrowseCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseKeyFileCommandInternal();
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

        if (folders.Count == 1)
        {
            dataContext.FolderFolder = folders[0].TryGetLocalPath() ?? folders[0].Path.ToString();
        }
    }

    private async Task BrowseKeyFileCommandInternal()
    {
        var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as WhereTaskViewModel ?? throw new NullReferenceException();
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.FolderFolder);
        var files = await root.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.KeyFile_Field,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        if (files.Count == 1)
        {
            dataContext.SftpKeyFile = files[0].TryGetLocalPath() ?? files[0].Path.ToString();
        }
    }
}
