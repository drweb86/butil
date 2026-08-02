using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class VersionsFilesTreeView : UserControl
{
    public VersionsFilesTreeView()
    {
        InitializeComponent();
    }

    public async Task BrowseRestoreFolderAsync()
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as RestoreVersionsViewModel ?? throw new NullReferenceException();
        if (dataContext.SelectedNode == null)
            return;

        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.Field_Folder,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        if (folders.Count == 1)
        {
            dataContext.RecoverTo(folders[0].TryGetLocalPath() ?? folders[0].Path.ToString());
        }
    }
}
