using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class WhenTaskView : UserControl
{
    public WhenTaskView()
    {
        InitializeComponent();

        DataContext = new WhenTaskViewModel(new BUtil.Core.Options.ScheduleInfo());
    }
}
