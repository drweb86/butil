using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class NameTaskView : UserControl
{
    public NameTaskView()
    {
        InitializeComponent();

        this.DataContext = new NameTaskViewModel(true, "Help text", "Some name");
    }
}
