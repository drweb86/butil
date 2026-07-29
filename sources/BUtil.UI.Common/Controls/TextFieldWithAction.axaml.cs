using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public partial class TextFieldWithAction : UserControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Label));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Text), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string?> ActionTextProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(ActionText));

    public static readonly StyledProperty<ICommand?> ActionCommandProperty =
        AvaloniaProperty.Register<TextFieldWithAction, ICommand?>(nameof(ActionCommand));

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Error));

    public static readonly StyledProperty<string?> HelpProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Help));

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<TextFieldWithAction, bool>(nameof(IsReadOnly));

    public static readonly StyledProperty<bool> IsMultilineProperty =
        AvaloniaProperty.Register<TextFieldWithAction, bool>(nameof(IsMultiline));

    public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
        AvaloniaProperty.Register<TextFieldWithAction, TextWrapping>(nameof(TextWrapping));

    public static readonly StyledProperty<double> TextBoxMinHeightProperty =
        AvaloniaProperty.Register<TextFieldWithAction, double>(nameof(TextBoxMinHeight));

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

    public string? ActionText
    {
        get => GetValue(ActionTextProperty);
        set => SetValue(ActionTextProperty, value);
    }

    public ICommand? ActionCommand
    {
        get => GetValue(ActionCommandProperty);
        set => SetValue(ActionCommandProperty, value);
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

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public bool IsMultiline
    {
        get => GetValue(IsMultilineProperty);
        set => SetValue(IsMultilineProperty, value);
    }

    public TextWrapping TextWrapping
    {
        get => GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }

    public double TextBoxMinHeight
    {
        get => GetValue(TextBoxMinHeightProperty);
        set => SetValue(TextBoxMinHeightProperty, value);
    }

    public TextFieldWithAction()
    {
        InitializeComponent();
    }
}
