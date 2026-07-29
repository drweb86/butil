using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public partial class IconButton : UserControl
{
    public static readonly StyledProperty<string?> IconProperty =
        AvaloniaProperty.Register<IconButton, string?>(nameof(Icon));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<IconButton, string?>(nameof(Text));

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<IconButton, ICommand?>(nameof(Command));

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

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public IconButton()
    {
        InitializeComponent();
    }
}
