using Avalonia;
using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class ExpanderHeader : UserControl
{
    public static readonly StyledProperty<string?> IconProperty =
        AvaloniaProperty.Register<ExpanderHeader, string?>(nameof(Icon));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<ExpanderHeader, string?>(nameof(Text));

    public string? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ExpanderHeader()
    {
        InitializeComponent();
    }
}
