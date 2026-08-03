using Avalonia;
using Avalonia.Controls;
using System.Collections;
using Loc = BUtil.Core.Localization.Resources;

namespace BUtil.UI.Controls;

public partial class RestoreFileBlameSection : UserControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<RestoreFileBlameSection, string?>(nameof(Title));

    public static readonly StyledProperty<IEnumerable?> ItemsProperty =
        AvaloniaProperty.Register<RestoreFileBlameSection, IEnumerable?>(nameof(Items));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Help { get; } = Loc.BackupVersion_FileVersion_Help;

    public IEnumerable? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public RestoreFileBlameSection()
    {
        InitializeComponent();
    }
}
