using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class TaskExecuterView : UserControl
{
    public TaskExecuterView()
    {
        InitializeComponent();

        DataContext = new TaskExecuterViewModel("error");
    }
}
