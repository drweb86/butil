using Avalonia.Controls;

namespace butil_ui.Controls;

public partial class EditIncrementalBackupTaskView : UserControl
{
    public EditIncrementalBackupTaskView()
    {
        InitializeComponent();
    }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((EditIncrementalBackupTaskViewModel)DataContext).Initialize();
        }
    }
}
