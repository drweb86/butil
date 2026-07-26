using Avalonia;
using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class MessageBar : UserControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<MessageBar, string?>(nameof(Text));

    public static readonly StyledProperty<MessageBarKind> KindProperty =
        AvaloniaProperty.Register<MessageBar, MessageBarKind>(nameof(Kind));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public MessageBarKind Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public MessageBar()
    {
        InitializeComponent();
        UpdateKind();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == KindProperty)
            UpdateKind();
    }

    private void UpdateKind()
    {
        BarBorder.Classes.Remove("success");
        BarBorder.Classes.Remove("error");
        BarBorder.Classes.Add(Kind == MessageBarKind.Success ? "success" : "error");
    }
}
