using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class ImportMediaTaskWhereTaskView : UserControl
{
    public ImportMediaTaskWhereTaskView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        DataContext = new ImportMediaTaskWhereTaskViewModel(
            "the folder",
            false,
            false,
            "transform file name",
            null,
            null,
            isNew: true);
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is ImportMediaTaskWhereTaskViewModel viewModel)
            viewModel.BrowseOutputFolderAsync = BrowseOutputFolderAsync;
    }

    private async Task BrowseOutputFolderAsync()
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as ImportMediaTaskWhereTaskViewModel ?? throw new NullReferenceException();
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.OutputFolder);
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.ImportMediaTask_Field_OutputFolder,
            SuggestedStartLocation = startLocation,
            AllowMultiple = false
        });

        if (folders.Count == 1)
        {
            dataContext.OutputFolder = folders[0].TryGetLocalPath() ?? folders[0].Path.ToString();
        }
    }
}
