using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System.Collections;

namespace BUtil.UI.Controls;

public partial class DropdownField : UserControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(Label));

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<DropdownField, IEnumerable?>(nameof(ItemsSource));

    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<DropdownField, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<DropdownField, IDataTemplate?>(nameof(ItemTemplate));

    public static readonly StyledProperty<string?> DisplayMemberProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(DisplayMember));

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(Error));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public string? DisplayMember
    {
        get => GetValue(DisplayMemberProperty);
        set => SetValue(DisplayMemberProperty, value);
    }

    public string? Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public DropdownField()
    {
        InitializeComponent();
        ApplyDisplayMember();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DisplayMemberProperty)
            ApplyDisplayMember();
    }

    private void ApplyDisplayMember()
    {
        if (InputComboBox is null)
            return;

        InputComboBox.DisplayMemberBinding = string.IsNullOrEmpty(DisplayMember)
            ? null
            : new Binding(DisplayMember);
    }
}
