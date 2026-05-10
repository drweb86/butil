using Avalonia.Controls;
using BUtil.UI;

namespace BUtil.Tasks.Synchronization.UI.Controls;

public partial class EditSynchronizationTaskView : UserControl, IViewLocatorAware<EditSynchronizationTaskViewModel>
{
    public EditSynchronizationTaskView()
    {
        InitializeComponent();
    }
}
