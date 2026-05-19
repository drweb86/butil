using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class TaskUIView : UserControl, IViewLocatorAware<TaskUIViewModel>
{
    public TaskUIView()
    {
        InitializeComponent();
    }
}
