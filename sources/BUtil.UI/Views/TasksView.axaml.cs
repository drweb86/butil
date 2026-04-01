using Avalonia.Controls;
using butil_ui.ViewModels;

namespace butil_ui.Views;

public partial class TasksView : UserControl
{
    public TasksView()
    {
        InitializeComponent();
    }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((TasksViewModel)DataContext).Initialize();
        }
    }
}
