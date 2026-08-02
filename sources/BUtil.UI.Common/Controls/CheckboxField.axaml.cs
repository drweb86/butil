using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace BUtil.UI.Controls;

public partial class CheckboxField : UserControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<CheckboxField, string?>(nameof(Label));

    public static readonly StyledProperty<bool> IsCheckedProperty =
        AvaloniaProperty.Register<CheckboxField, bool>(nameof(IsChecked), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> HelpProperty =
        AvaloniaProperty.Register<CheckboxField, string?>(nameof(Help));

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<CheckboxField, string?>(nameof(Error));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
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

    public CheckboxField()
    {
        InitializeComponent();
    }
}
