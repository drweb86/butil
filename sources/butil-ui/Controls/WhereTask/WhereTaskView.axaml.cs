using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class WhereTaskView : UserControl
    {
        public WhereTaskView()
        {
            InitializeComponent();

            var viewModel = new Controls.WhereTaskViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
