using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BUtil.Core.Misc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class TechnicalFileToolView : UserControl, IViewLocatorAware<TechnicalFileToolViewModel>
{
    public TechnicalFileToolView()
    {
        InitializeComponent();
    }

    private void BrowseSourceClick(object? sender, RoutedEventArgs e)
    {
        _ = BrowseSourceAsync();
    }

    private void BrowseOutputClick(object? sender, RoutedEventArgs e)
    {
        _ = BrowseOutputAsync();
    }

    private async Task BrowseSourceAsync()
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new InvalidOperationException();
        var vm = DataContext as TechnicalFileToolViewModel ?? throw new InvalidOperationException();
        var folder = Path.GetDirectoryName(vm.InputPath);
        var start = !string.IsNullOrEmpty(folder)
            ? await root.StorageProvider.TryGetFolderFromPathAsync(folder)
            : null;

        var files = await root.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = vm.PickSourceTitle,
            AllowMultiple = false,
            SuggestedStartLocation = start,
            FileTypeFilter = vm.GetSourceFileTypeFilters().ToList(),
        });

        if (files.Count == 1)
            vm.InputPath = files[0].TryGetLocalPath() ?? files[0].Path.ToString();
    }

    private async Task BrowseOutputAsync()
    {
        var root = TopLevel.GetTopLevel(this) ?? throw new InvalidOperationException();
        var vm = DataContext as TechnicalFileToolViewModel ?? throw new InvalidOperationException();
        var folder = Path.GetDirectoryName(vm.OutputPath);
        if (string.IsNullOrEmpty(folder))
            folder = Path.GetDirectoryName(vm.InputPath);
        var start = !string.IsNullOrEmpty(folder)
            ? await root.StorageProvider.TryGetFolderFromPathAsync(folder)
            : null;

        var file = await root.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = vm.PickOutputTitle,
            SuggestedStartLocation = start,
            SuggestedFileName = vm.SuggestedSaveFileName,
            FileTypeChoices = vm.GetSaveFileTypeFilters().ToList(),
            DefaultExtension = vm.Kind switch
            {
                TechnicalFileToolKind.EncryptAes256 => SourceItemHelper.AES256V1Extension,
                TechnicalFileToolKind.CompressBrotli => TechnicalFileToolViewModel.BrotliExtensionNoDot,
                _ => null,
            },
        });

        if (file != null)
            vm.OutputPath = file.TryGetLocalPath() ?? file.Path.ToString();
    }
}
