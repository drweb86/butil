using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class UpdateCheckExpanderView : UserControl
{
    public UpdateCheckExpanderView()
    {
        InitializeComponent();

        var viewModel = new UpdateCheckExpanderViewModel();
        DataContext = viewModel;
        viewModel.Initialize();
    }
}
