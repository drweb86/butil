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

    public DialogButtons()
    {
        InitializeComponent();
    }
}
