using Avalonia;
using Avalonia.Controls;
using System.Collections;

namespace BUtil.UI.Controls;

public partial class RestoreVersionChanges : UserControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<RestoreVersionChanges, string?>(nameof(Title));

    public static readonly StyledProperty<IEnumerable?> ItemsProperty =
        AvaloniaProperty.Register<RestoreVersionChanges, IEnumerable?>(nameof(Items));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public IEnumerable? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public RestoreVersionChanges()
    {
        InitializeComponent();
    }
}
