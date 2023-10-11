using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class EncryptionTaskView : UserControl
    {
        public EncryptionTaskView()
        {
            InitializeComponent();

            var viewModel = new Controls.EncryptionTaskViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
