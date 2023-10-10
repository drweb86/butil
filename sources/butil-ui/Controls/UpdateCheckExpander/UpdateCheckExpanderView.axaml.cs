using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class UpdateCheckExpanderView : UserControl
    {
        public UpdateCheckExpanderView()
        {
            InitializeComponent();

            var viewModel = new Controls.UpdateCheckExpander.UpdateCheckExpanderViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
