using Avalonia.Controls;
using Avalonia.Platform.Storage;
using BUtil.Tasks.BUtilServer;
using System;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class FtpsServerConfigurationView : UserControl
{
    public FtpsServerConfigurationView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        DataContext = new FtpsServerConfigurationViewModel(
            BUtilServerModelOptionsV2.DefaultPort,
            BUtilServerModelOptionsV2.DefaultUsername,
            "pass",
            "some folder",
            BUtilServerModelOptionsV2.DefaultDuration);
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is FtpsServerConfigurationViewModel viewModel)
            viewModel.BrowseFolderAsync = BrowseFolderAsync;
    }

    private async Task BrowseFolderAsync()
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as FtpsServerConfigurationViewModel ?? throw new NullReferenceException();
        IStorageFolder? startLocation = null;
        try
        {
            startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.Folder);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.Field_Folder,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        if (folders.Count == 1)
        {
            dataContext.Folder = folders[0].TryGetLocalPath() ?? folders[0].Path.ToString();
        }
    }
}
