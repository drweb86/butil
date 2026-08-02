using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public partial class ConfirmPanel : UserControl
{
    public static readonly StyledProperty<string?> MessageProperty =
        AvaloniaProperty.Register<ConfirmPanel, string?>(nameof(Message));

    public static readonly StyledProperty<string?> ConfirmTextProperty =
        AvaloniaProperty.Register<ConfirmPanel, string?>(nameof(ConfirmText));

    public static readonly StyledProperty<ICommand?> ConfirmCommandProperty =
        AvaloniaProperty.Register<ConfirmPanel, ICommand?>(nameof(ConfirmCommand));

    public static readonly StyledProperty<string?> CancelTextProperty =
        AvaloniaProperty.Register<ConfirmPanel, string?>(nameof(CancelText));

    public static readonly StyledProperty<ICommand?> CancelCommandProperty =
        AvaloniaProperty.Register<ConfirmPanel, ICommand?>(nameof(CancelCommand));

    public string? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public string? ConfirmText
    {
        get => GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    public ICommand? ConfirmCommand
    {
        get => GetValue(ConfirmCommandProperty);
        set => SetValue(ConfirmCommandProperty, value);
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

    public ConfirmPanel()
    {
        InitializeComponent();
    }
}
