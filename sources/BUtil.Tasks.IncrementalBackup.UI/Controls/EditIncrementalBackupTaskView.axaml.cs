using Avalonia.Controls;
using BUtil.UI;

namespace BUtil.Tasks.IncrementalBackup.UI.Controls;

public partial class EditIncrementalBackupTaskView : UserControl, IViewLocatorAware<EditIncrementalBackupTaskViewModel>
{
    public EditIncrementalBackupTaskView()
    {
        InitializeComponent();
    }
}
