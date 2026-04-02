using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class LaunchTaskView : UserControl, IViewLocatorAware<LaunchTaskViewModel>
{
    public LaunchTaskView()
    {
        InitializeComponent();
    }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((LaunchTaskViewModel)DataContext).Initialize();
        }
    }
}
