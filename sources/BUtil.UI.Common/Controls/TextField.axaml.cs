using Avalonia;
using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class TextField : UserControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Label));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Text), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string?> PlaceholderProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Placeholder));

    public static readonly StyledProperty<char> PasswordCharProperty =
        AvaloniaProperty.Register<TextField, char>(nameof(PasswordChar));

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Error));

    public static readonly StyledProperty<string?> HelpProperty =
        AvaloniaProperty.Register<TextField, string?>(nameof(Help));

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

    public string? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    public string? Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public string? Help
    {
        get => GetValue(HelpProperty);
        set => SetValue(HelpProperty, value);
    }

    public TextField()
    {
        InitializeComponent();
    }
}
