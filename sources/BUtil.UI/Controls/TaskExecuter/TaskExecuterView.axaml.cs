using Avalonia.Controls;

namespace butil_ui.Controls;

public partial class TaskExecuterView : UserControl
{
    public TaskExecuterView()
    {
        InitializeComponent();

        DataContext = new TaskExecuterViewModel("error");
    }
}
