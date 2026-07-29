using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public partial class WhatTaskView : UserControl
{
    public ICommand BrowseFoldersCommand { get; }

    public WhatTaskView()
    {
        BrowseFoldersCommand = new RelayCommand(() => _ = BrowseFoldersCommandInternal());

        InitializeComponent();

        DataContext = new WhatTaskViewModel([], []);
    }

    private async Task BrowseFoldersCommandInternal()
    {
        try
        {
            var root = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");
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
                dataContext.AddFolder(folder.TryGetLocalPath() ?? folder.Path.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
