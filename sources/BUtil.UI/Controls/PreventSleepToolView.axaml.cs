using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BUtil.UI.Controls;

public partial class PreventSleepToolView : UserControl, IViewLocatorAware<PreventSleepToolViewModel>
{
    public PreventSleepToolView()
    {
        InitializeComponent();
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is PreventSleepToolViewModel viewModel)
            viewModel.StopCommand();
    }
}
