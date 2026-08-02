using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Threading.Tasks;

namespace BUtil.Tasks.BUtilClient.UI.Controls;

public partial class FolderSectionView : UserControl
{
    public FolderSectionView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        DataContext = new FolderSectionViewModel("the folder", false, false);
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is FolderSectionViewModel viewModel)
            viewModel.BrowseFolderAsync = BrowseFolderAsync;
    }

    private async Task BrowseFolderAsync()
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as FolderSectionViewModel ?? throw new NullReferenceException();
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
