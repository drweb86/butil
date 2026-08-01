using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace BUtil.UI.Controls;

public partial class NumericUpDownField : UserControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<NumericUpDownField, string?>(nameof(Label));

    public static readonly StyledProperty<long> ValueProperty =
        AvaloniaProperty.Register<NumericUpDownField, long>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<long> MinimumProperty =
        AvaloniaProperty.Register<NumericUpDownField, long>(nameof(Minimum));

    public static readonly StyledProperty<string?> HelpProperty =
        AvaloniaProperty.Register<NumericUpDownField, string?>(nameof(Help));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public long Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public long Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public string? Help
    {
        get => GetValue(HelpProperty);
        set => SetValue(HelpProperty, value);
    }

    public NumericUpDownField()
    {
        InitializeComponent();
    }
}
