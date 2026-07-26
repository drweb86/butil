using Avalonia;
using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class TextField : UserControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Label));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Text), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Error));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string? Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public TextField()
    {
        InitializeComponent();
    }
}
