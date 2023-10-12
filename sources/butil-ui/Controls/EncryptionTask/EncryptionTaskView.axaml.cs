using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class EncryptionTaskView : UserControl
    {
        public EncryptionTaskView()
        {
            InitializeComponent();

            this.DataContext = new EncryptionTaskViewModel("password");
        }
    }
}
