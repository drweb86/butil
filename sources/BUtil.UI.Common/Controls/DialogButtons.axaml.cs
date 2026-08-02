using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public partial class DialogButtons : UserControl
{
    public static readonly StyledProperty<string?> PrimaryTextProperty =
        AvaloniaProperty.Register<DialogButtons, string?>(nameof(PrimaryText));

    public static readonly StyledProperty<ICommand?> PrimaryCommandProperty =
        AvaloniaProperty.Register<DialogButtons, ICommand?>(nameof(PrimaryCommand));

    public static readonly StyledProperty<string?> CancelTextProperty =
        AvaloniaProperty.Register<DialogButtons, string?>(nameof(CancelText));

    public static readonly StyledProperty<ICommand?> CancelCommandProperty =
        AvaloniaProperty.Register<DialogButtons, ICommand?>(nameof(CancelCommand));

    public static readonly StyledProperty<string?> MessageTextProperty =
        AvaloniaProperty.Register<DialogButtons, string?>(nameof(MessageText));

    public static readonly StyledProperty<MessageBarKind> MessageKindProperty =
        AvaloniaProperty.Register<DialogButtons, MessageBarKind>(nameof(MessageKind));

    public static readonly StyledProperty<bool> IsPrimaryVisibleProperty =
        AvaloniaProperty.Register<DialogButtons, bool>(nameof(IsPrimaryVisible), true);

    public string? PrimaryText
    {
        get => GetValue(PrimaryTextProperty);
        set => SetValue(PrimaryTextProperty, value);
    }

    public ICommand? PrimaryCommand
    {
        get => GetValue(PrimaryCommandProperty);
        set => SetValue(PrimaryCommandProperty, value);
    }

    public string? CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public ICommand? CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public string? MessageText
    {
        get => GetValue(MessageTextProperty);
        set => SetValue(MessageTextProperty, value);
    }

    public MessageBarKind MessageKind
    {
        get => GetValue(MessageKindProperty);
        set => SetValue(MessageKindProperty, value);
    }

    public bool IsPrimaryVisible
    {
        get => GetValue(IsPrimaryVisibleProperty);
        set => SetValue(IsPrimaryVisibleProperty, value);
    }

    public DialogButtons()
    {
        InitializeComponent();
    }
}
