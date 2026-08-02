using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class VersionsListView : UserControl
{
    public VersionsListView()
    {
        InitializeComponent();
        DataContext = new VersionsListViewModel(null!);
    }
}
