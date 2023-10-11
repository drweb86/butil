using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class ImportMediaTaskWhereTaskView : UserControl
    {
        public ImportMediaTaskWhereTaskView()
        {
            InitializeComponent();

            var viewModel = new Controls.ImportMediaTaskWhereTaskViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
