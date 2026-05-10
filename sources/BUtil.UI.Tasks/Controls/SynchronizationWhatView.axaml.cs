using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class SynchronizationWhatView : UserControl
{
    public SynchronizationWhatView()
    {
        InitializeComponent();
        this.DataContext = new SynchronizationWhatViewModel("the folder", BUtil.Core.ConfigurationFileModels.V2.SynchronizationTaskModelMode.TwoWay);
    }

    public void BrowseCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseCommandInternal();
    }

    private async Task BrowseCommandInternal()
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as SynchronizationWhatViewModel ?? throw new NullReferenceException();
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
