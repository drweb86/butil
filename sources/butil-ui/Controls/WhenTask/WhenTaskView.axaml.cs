using Avalonia.Controls;

namespace butil_ui.Controls;

public partial class WhenTaskView : UserControl
{
    public WhenTaskView()
    {
        InitializeComponent();

        this.DataContext = new WhenTaskViewModel(new BUtil.Core.Options.ScheduleInfo());
    }
}
