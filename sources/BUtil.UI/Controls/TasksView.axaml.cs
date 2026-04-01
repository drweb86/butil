using Avalonia.Controls;

namespace BUtil.UI.Controls;

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
