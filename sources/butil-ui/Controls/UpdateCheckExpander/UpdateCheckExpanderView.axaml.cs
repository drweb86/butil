using Avalonia.Controls;

namespace butil_ui.Controls;

public partial class UpdateCheckExpanderView : UserControl
{
    public UpdateCheckExpanderView()
    {
        InitializeComponent();

        var viewModel = new UpdateCheckExpander.UpdateCheckExpanderViewModel();
        DataContext = viewModel;
        viewModel.Initialize();
    }
}
