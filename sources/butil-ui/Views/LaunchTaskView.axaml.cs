using Avalonia.Controls;
using butil_ui.ViewModels;

namespace butil_ui.Views;

public partial class LaunchTaskView : UserControl
{
    public LaunchTaskView()
    {
        InitializeComponent();
    }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ((LaunchTaskViewModel)this.DataContext).Load();
    }
}
