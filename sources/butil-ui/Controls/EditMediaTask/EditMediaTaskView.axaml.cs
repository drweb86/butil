using Avalonia.Controls;

namespace butil_ui.Controls;

public partial class EditMediaTaskView : UserControl
{
    public EditMediaTaskView()
    {
        InitializeComponent();
    }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((EditMediaTaskViewModel)DataContext).Initialize();
        }
    }
}
