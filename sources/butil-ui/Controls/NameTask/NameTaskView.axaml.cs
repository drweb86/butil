using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class NameTaskView : UserControl
    {
        public NameTaskView()
        {
            InitializeComponent();

            var viewModel = new Controls.NameTaskViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
