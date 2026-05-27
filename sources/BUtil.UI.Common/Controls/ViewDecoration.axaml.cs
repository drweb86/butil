using Avalonia;
using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class ViewDecoration : UserControl
{
    public static readonly StyledProperty<ViewDecorationKind> KindProperty =
        AvaloniaProperty.Register<ViewDecoration, ViewDecorationKind>(nameof(Kind));

    public ViewDecorationKind Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public ViewDecoration()
    {
        InitializeComponent();
        UpdateGeometry(Kind);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == KindProperty)
            UpdateGeometry(Kind);
    }

    private void UpdateGeometry(ViewDecorationKind kind) =>
        DecorationPath.Data = ViewDecorationGeometries.Get(kind);
}
