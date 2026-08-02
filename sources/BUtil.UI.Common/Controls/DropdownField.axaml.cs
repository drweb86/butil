using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System.Collections;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public partial class DropdownField : UserControl
{
    private const double MinComboBoxWidthForSideAction = 400;

    private double _lastRightActionButtonWidth;
    private bool _textBound;
    private bool _isActionBelow;

    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(Label));

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<DropdownField, IEnumerable?>(nameof(ItemsSource));

    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<DropdownField, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(Text), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsEditableProperty =
        AvaloniaProperty.Register<DropdownField, bool>(nameof(IsEditable));

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<DropdownField, IDataTemplate?>(nameof(ItemTemplate));

    public static readonly StyledProperty<string?> DisplayMemberProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(DisplayMember));

    public static readonly StyledProperty<string?> HelpProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(Help));

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(Error));

    public static readonly StyledProperty<string?> ActionTextProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(ActionText));

    public static readonly StyledProperty<string?> ActionIconProperty =
        AvaloniaProperty.Register<DropdownField, string?>(nameof(ActionIcon));

    public static readonly StyledProperty<ICommand?> ActionCommandProperty =
        AvaloniaProperty.Register<DropdownField, ICommand?>(nameof(ActionCommand));

    public static readonly StyledProperty<bool> ActionIsEnabledProperty =
        AvaloniaProperty.Register<DropdownField, bool>(nameof(ActionIsEnabled), true);

    public static readonly StyledProperty<bool> ShowSideActionProperty =
        AvaloniaProperty.Register<DropdownField, bool>(nameof(ShowSideAction));

    public static readonly StyledProperty<bool> ShowBelowActionProperty =
        AvaloniaProperty.Register<DropdownField, bool>(nameof(ShowBelowAction));

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

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsEditable
    {
        get => GetValue(IsEditableProperty);
        set => SetValue(IsEditableProperty, value);
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

    public string? Help
    {
        get => GetValue(HelpProperty);
        set => SetValue(HelpProperty, value);
    }

    public string? Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public string? ActionText
    {
        get => GetValue(ActionTextProperty);
        set => SetValue(ActionTextProperty, value);
    }

    public string? ActionIcon
    {
        get => GetValue(ActionIconProperty);
        set => SetValue(ActionIconProperty, value);
    }

    public ICommand? ActionCommand
    {
        get => GetValue(ActionCommandProperty);
        set => SetValue(ActionCommandProperty, value);
    }

    public bool ActionIsEnabled
    {
        get => GetValue(ActionIsEnabledProperty);
        set => SetValue(ActionIsEnabledProperty, value);
    }

    public bool ShowSideAction
    {
        get => GetValue(ShowSideActionProperty);
        private set => SetValue(ShowSideActionProperty, value);
    }

    public bool ShowBelowAction
    {
        get => GetValue(ShowBelowActionProperty);
        private set => SetValue(ShowBelowActionProperty, value);
    }

    public DropdownField()
    {
        InitializeComponent();
        ApplyDisplayMember();
        UpdateTextBinding();
        UpdateActionPlacement();

        InputComboBox.PropertyChanged += (_, e) =>
        {
            if (e.Property == BoundsProperty)
                UpdateActionPlacement();
        };

        RightActionButton.PropertyChanged += (_, e) =>
        {
            if (e.Property == BoundsProperty)
            {
                var width = RightActionButton.Bounds.Width;
                if (width > 0)
                    _lastRightActionButtonWidth = width;
                UpdateActionPlacement();
            }
        };
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DisplayMemberProperty)
            ApplyDisplayMember();

        if (change.Property == IsEditableProperty)
            UpdateTextBinding();

        if (change.Property == ActionCommandProperty
            || change.Property == ActionTextProperty
            || change.Property == ActionIconProperty
            || change.Property == BoundsProperty)
        {
            UpdateActionPlacement();
        }
    }

    private void ApplyDisplayMember()
    {
        if (InputComboBox is null)
            return;

        InputComboBox.DisplayMemberBinding = string.IsNullOrEmpty(DisplayMember)
            ? null
            : new Binding(DisplayMember);
    }

    private void UpdateTextBinding()
    {
        if (InputComboBox is null)
            return;

        if (IsEditable)
        {
            if (_textBound)
                return;

            InputComboBox.Bind(ComboBox.TextProperty, new Binding
            {
                Path = nameof(Text),
                Source = this,
                Mode = BindingMode.TwoWay,
            });
            _textBound = true;
        }
        else if (_textBound)
        {
            InputComboBox.ClearValue(ComboBox.TextProperty);
            _textBound = false;
        }
    }

    private void UpdateActionPlacement()
    {
        if (InputComboBox is null)
            return;

        var hasAction = ActionCommand != null
            || !string.IsNullOrEmpty(ActionText)
            || !string.IsNullOrEmpty(ActionIcon);

        _isActionBelow = hasAction && GetSideComboBoxWidth() < MinComboBoxWidthForSideAction;
        ShowSideAction = hasAction && !_isActionBelow;
        ShowBelowAction = hasAction && _isActionBelow;
    }

    private double GetSideComboBoxWidth()
    {
        if (!_isActionBelow)
            return InputComboBox.Bounds.Width;

        var actionWidth = _lastRightActionButtonWidth;
        if (actionWidth <= 0)
            return InputComboBox.Bounds.Width;

        return Bounds.Width - actionWidth - InputComboBox.Margin.Right;
    }
}
