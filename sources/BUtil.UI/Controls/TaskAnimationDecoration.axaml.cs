using Avalonia;
using Avalonia.Controls;
using System;

namespace BUtil.UI.Controls;

/// <summary>
/// Hosts the decorative animation control for the collapsed task execution view. The
/// concrete control is supplied by the task's UI plugin via <see cref="Factory"/> (see
/// <c>TaskUIProviderRegistry.GetAnimationFactory</c>), so 3rd-party task plugins can provide
/// their own animation without this project knowing about their type.
/// </summary>
public partial class TaskAnimationDecoration : ContentControl
{
    public static readonly StyledProperty<Func<object>?> FactoryProperty =
        AvaloniaProperty.Register<TaskAnimationDecoration, Func<object>?>(nameof(Factory));

    public Func<object>? Factory
    {
        get => GetValue(FactoryProperty);
        set => SetValue(FactoryProperty, value);
    }

    public TaskAnimationDecoration()
    {
        InitializeComponent();
        UpdateContent(Factory);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == FactoryProperty)
            UpdateContent(Factory);
    }

    private void UpdateContent(Func<object>? factory) => Content = factory?.Invoke() as Control;
}
