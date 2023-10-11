using Avalonia.Controls;
using butil_ui.ViewModels;

namespace butil_ui.Views;

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
