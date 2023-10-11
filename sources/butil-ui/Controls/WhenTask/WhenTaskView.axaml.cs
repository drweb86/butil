using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class WhenTaskView : UserControl
    {
        public WhenTaskView()
        {
            InitializeComponent();

            var viewModel = new Controls.WhenTaskViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
