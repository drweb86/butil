using Avalonia.Controls;

namespace butil_ui.Controls;

public partial class WhenTaskView : UserControl
{
    public WhenTaskView()
    {
        InitializeComponent();

        DataContext = new WhenTaskViewModel(new BUtil.Core.Options.ScheduleInfo());
    }
}
