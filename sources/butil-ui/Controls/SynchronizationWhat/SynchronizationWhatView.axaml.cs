using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public partial class SynchronizationWhatView : UserControl
{
    public SynchronizationWhatView()
    {
        InitializeComponent();
        this.DataContext = new FolderSectionViewModel("the folder");
    }

    public void BrowseCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseCommandInternal();
    }

    private async Task BrowseCommandInternal()
    {
        var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as FolderSectionViewModel ?? throw new NullReferenceException();
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.Folder);
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.ImportMediaTask_Field_OutputFolder,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        var folder = folders.FirstOrDefault();
        if (folder != null)
        {
            dataContext.Folder = folder.TryGetLocalPath() ?? folder.Path.ToString();
        }
    }
}
