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

        DataContext = new WhatTaskViewModel([], []);
    }

    public void BrowseFoldersCommand(object? sender, RoutedEventArgs args)
    {
        _ = BrowseFoldersCommandInternal();
    }

    private async Task BrowseFoldersCommandInternal()
    {
        try
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
        catch (Exception e)
        { 
            Console.WriteLine(e.ToString());
        }
    }


}
