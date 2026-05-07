using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.UI.Controls.StorageFields;
using System;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class WhereTaskView : UserControl
{
    public WhereTaskView()
    {
        InitializeComponent();
        DataContext = new WhereTaskViewModel(
            new FolderStorageSettingsV2(),
            BUtil.Core.Localization.Resources.LeftMenu_Where,
            "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
    }

    public void FieldBrowseFolderCommand(object? sender, RoutedEventArgs args)
    {
        if ((sender as Button)?.DataContext is FolderFieldViewModel vm)
            _ = BrowseFolderAsync(vm);
    }

    public void FieldBrowseFileCommand(object? sender, RoutedEventArgs args)
    {
        if ((sender as Button)?.DataContext is FileFieldViewModel vm)
            _ = BrowseFileAsync(vm);
    }

    public void MountScriptLaunchCommand(object? sender, RoutedEventArgs args) =>
        _ = ((WhereTaskViewModel)DataContext!).MountTaskLaunchCommand();

    public void UnmountScriptLaunchCommand(object? sender, RoutedEventArgs args) =>
        _ = ((WhereTaskViewModel)DataContext!).UnmountTaskLaunchCommand();

    private async Task BrowseFolderAsync(FolderFieldViewModel vm)
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new InvalidOperationException("No TopLevel found.");
        var start = await root.StorageProvider.TryGetFolderFromPathAsync(vm.Value);
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = BUtil.Core.Localization.Resources.Field_Folder,
            SuggestedStartLocation = start,
            AllowMultiple = false,
        });
        if (folders.Count == 1)
            vm.Value = folders[0].TryGetLocalPath() ?? folders[0].Path.ToString();
    }

    private async Task BrowseFileAsync(FileFieldViewModel vm)
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new InvalidOperationException("No TopLevel found.");
        var start = await root.StorageProvider.TryGetFolderFromPathAsync(vm.Value);
        var files = await root.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = BUtil.Core.Localization.Resources.KeyFile_Field,
            SuggestedStartLocation = start,
            AllowMultiple = false,
        });
        if (files.Count == 1)
            vm.Value = files[0].TryGetLocalPath() ?? files[0].Path.ToString();
    }
}
