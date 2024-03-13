using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public partial class WhatTaskView : UserControl
{
    public WhatTaskView()
    {
        InitializeComponent();

        this.DataContext = new WhatTaskViewModel(new System.Collections.Generic.List<BUtil.Core.ConfigurationFileModels.V2.SourceItemV2> { }, new System.Collections.Generic.List<string> { });
    }

    public void BrowseFilesCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseFilesCommandInternal();
    }

    private async Task BrowseFilesCommandInternal()
    {
        var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as WhatTaskViewModel ?? throw new NullReferenceException();
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        var files = await root.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.SourceItem_AddFiles,
            SuggestedStartLocation = startLocation,
            AllowMultiple = true
        });

        foreach (var file in files)
        {
            dataContext.Items.Add(new SourceItemV2ViewModel(Guid.NewGuid(),
                file.TryGetLocalPath() ?? file.Path.ToString(),
                false,
                dataContext._items));
        }
    }

    public void BrowseFoldersCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseFoldersCommandInternal();
    }

    private async Task BrowseFoldersCommandInternal()
    {
        var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        var dataContext = DataContext as WhatTaskViewModel ?? throw new NullReferenceException();
        var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = BUtil.Core.Localization.Resources.SourceItem_AddFolders,
            SuggestedStartLocation = startLocation,
            AllowMultiple = true
        });

        foreach (var folder in folders)
        {
            dataContext.Items.Add(new SourceItemV2ViewModel(Guid.NewGuid(),
                folder.TryGetLocalPath() ?? folder.Path.ToString(),
                true,
                dataContext._items));
        }
    }


}
