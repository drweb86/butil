using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BUtil.UI.Controls;

public partial class WhenTaskView : UserControl
{
    public WhenTaskView()
    {
        InitializeComponent();

        DataContext = new WhenTaskViewModel(new BUtil.Core.Options.ScheduleInfo());

        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        var dayCheckBoxes = DaysPanel.Children.OfType<CheckBox>().ToArray();
        var uniformWidth = Math.Ceiling(dayCheckBoxes.Max(checkBox =>
        {
            checkBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return checkBox.DesiredSize.Width;
        }));

        foreach (var checkBox in dayCheckBoxes)
            checkBox.Width = uniformWidth;
    }
}
