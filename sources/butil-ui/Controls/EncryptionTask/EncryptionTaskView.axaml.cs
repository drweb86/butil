using Avalonia.Controls;

namespace butil_ui.Controls;

public partial class EncryptionTaskView : UserControl
{
    public EncryptionTaskView()
    {
        InitializeComponent();

        DataContext = new EncryptionTaskViewModel("password");
    }
}
