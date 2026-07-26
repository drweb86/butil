using Avalonia;
using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class ValidationError : UserControl
{
    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<ValidationError, string?>(nameof(Error));

    public string? Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public ValidationError()
    {
        InitializeComponent();
    }
}
