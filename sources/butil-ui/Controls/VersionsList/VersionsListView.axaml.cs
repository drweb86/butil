using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BUtil.Core.ConfigurationFileModels.V2;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace butil_ui.Controls
{
    public partial class VersionsListView : UserControl
    {
        public VersionsListView()
        {
            InitializeComponent();
            DataContext = new VersionsListViewModel();
        }

        public void BrowseCommand(object? sender, RoutedEventArgs args)
        {
            _ = BrowseCommandInternal();
        }

        private async Task BrowseCommandInternal()
        {
            var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
            var dataContext = DataContext as WhereTaskViewModel ?? throw new NullReferenceException();
            var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.FolderFolder);
            var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = BUtil.Core.Localization.Resources.Field_Folder,
                SuggestedStartLocation = startLocation,
                AllowMultiple = false
            });

            var folder = folders.FirstOrDefault();
            if (folder != null)
            {
                dataContext.FolderFolder = folder.TryGetLocalPath() ?? folder.Path.ToString();
            }
        }
    }
}