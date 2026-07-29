using Avalonia;
using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class FormHeader : UserControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<FormHeader, string?>(nameof(Text));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public FormHeader()
    {
        InitializeComponent();
    }
}
