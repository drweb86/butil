using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class ImportDataSelectionView : UserControl
{
    public ImportDataSelectionView()
    {
        InitializeComponent();
        DataContext = new ImportDataSelectionViewModel(
            skipAlreadyImportedFiles: false,
            deleteCopiedDataOnSourceMedia: false,
            fileLastWriteTimeMin: null,
            fileExtensions: null,
            isNew: true);
    }
}