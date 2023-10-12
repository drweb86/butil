using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace butil_ui.Controls
{
    public partial class ImportMediaTaskWhereTaskView : UserControl
    {
        public ImportMediaTaskWhereTaskView()
        {
            InitializeComponent();

            this.DataContext = new ImportMediaTaskWhereTaskViewModel("the folder", false, "transoform file name");
        }

        public void BrowseCommand(object? sender, RoutedEventArgs args)
        {
            _ = BrowseCommandInternal();
        }

        private async Task BrowseCommandInternal()
        {
            var root = this.VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
            var dataContext = DataContext as ImportMediaTaskWhereTaskViewModel ?? throw new NullReferenceException();
            var startLocation = await root.StorageProvider.TryGetFolderFromPathAsync(dataContext.OutputFolder);
            var folders = await root.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = BUtil.Core.Localization.Resources.ImportMediaTask_Field_OutputFolder,
                SuggestedStartLocation = startLocation,
                AllowMultiple = false
            });

            var folder = folders.FirstOrDefault();
            if (folder != null)
            {
                dataContext.OutputFolder = folder.TryGetLocalPath() ?? folder.Path.ToString();
            }
        }

    }
}
