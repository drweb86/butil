using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;

namespace BUtil.UI.Controls;

public partial class DateField : UserControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<DateField, string?>(nameof(Label));

    public static readonly StyledProperty<DateTime?> SelectedDateProperty =
        AvaloniaProperty.Register<DateField, DateTime?>(nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> HelpProperty =
        AvaloniaProperty.Register<DateField, string?>(nameof(Help));

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<DateField, string?>(nameof(Error));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public string? Help
    {
        get => GetValue(HelpProperty);
        set => SetValue(HelpProperty, value);
    }

    public string? Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public DateField()
    {
        InitializeComponent();
    }
}
