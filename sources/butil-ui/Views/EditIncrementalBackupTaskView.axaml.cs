using Avalonia.Controls;
using butil_ui.ViewModels;

namespace butil_ui.Views;

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
