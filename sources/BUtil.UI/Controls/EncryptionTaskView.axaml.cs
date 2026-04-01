using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class EncryptionTaskView : UserControl
{
    public EncryptionTaskView()
    {
        InitializeComponent();

        DataContext = new EncryptionTaskViewModel("password");
    }
}
