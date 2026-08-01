using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Windows.Input;

namespace BUtil.UI.Controls;

public partial class TextFieldWithAction : UserControl
{
    private const double MinTextBoxWidthForSideAction = 400;

    private double _lastRightActionButtonWidth;

    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Label));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Text), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<string?> PlaceholderProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Placeholder));

    public static readonly StyledProperty<string?> ActionTextProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(ActionText));

    public static readonly StyledProperty<string?> ActionIconProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(ActionIcon));

    public static readonly StyledProperty<ICommand?> ActionCommandProperty =
        AvaloniaProperty.Register<TextFieldWithAction, ICommand?>(nameof(ActionCommand));

    public static readonly StyledProperty<string?> ErrorProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Error));

    public static readonly StyledProperty<string?> HelpProperty =
        AvaloniaProperty.Register<TextFieldWithAction, string?>(nameof(Help));

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<TextFieldWithAction, bool>(nameof(IsReadOnly));

    public static readonly StyledProperty<bool> IsMultilineProperty =
        AvaloniaProperty.Register<TextFieldWithAction, bool>(nameof(IsMultiline));

    public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
        AvaloniaProperty.Register<TextFieldWithAction, TextWrapping>(nameof(TextWrapping));

    public static readonly StyledProperty<double> TextBoxMinHeightProperty =
        AvaloniaProperty.Register<TextFieldWithAction, double>(nameof(TextBoxMinHeight));

    public static readonly StyledProperty<bool> IsActionBelowProperty =
        AvaloniaProperty.Register<TextFieldWithAction, bool>(nameof(IsActionBelow));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
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

    public string? Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public string? Help
    {
        get => GetValue(HelpProperty);
        set => SetValue(HelpProperty, value);
    }

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public bool IsMultiline
    {
        get => GetValue(IsMultilineProperty);
        set
        {
            SetValue(IsMultilineProperty, value);
            UpdateActionPlacement();
        }
    }

    public TextWrapping TextWrapping
    {
        get => GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }

    public double TextBoxMinHeight
    {
        get => GetValue(TextBoxMinHeightProperty);
        set => SetValue(TextBoxMinHeightProperty, value);
    }

    public bool IsActionBelow
    {
        get => GetValue(IsActionBelowProperty);
        private set => SetValue(IsActionBelowProperty, value);
    }

    public TextFieldWithAction()
    {
        InitializeComponent();

        InputTextBox.PropertyChanged += (_, e) =>
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

        if (change.Property == IsMultilineProperty || change.Property == BoundsProperty)
            UpdateActionPlacement();
    }

    private void UpdateActionPlacement()
    {
        if (InputTextBox is null)
            return;

        IsActionBelow = IsMultiline || GetSideTextBoxWidth() < MinTextBoxWidthForSideAction;
    }

    private double GetSideTextBoxWidth()
    {
        if (!IsActionBelow)
            return InputTextBox.Bounds.Width;

        var actionWidth = _lastRightActionButtonWidth;
        if (actionWidth <= 0)
            return InputTextBox.Bounds.Width;

        return Bounds.Width - actionWidth - InputTextBox.Margin.Right;
    }
}
