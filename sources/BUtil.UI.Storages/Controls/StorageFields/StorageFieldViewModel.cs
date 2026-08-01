using BUtil.Core.Localization;
using BUtil.Core.Storages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BUtil.UI.Controls.StorageFields;

public abstract class StorageFieldViewModel(StorageFieldDescriptor descriptor) : ObservableObject
{
    private string? _uiLabelOverride;
    private bool _uiHidden;
    private string? _uiPlaceholderOverride;
    private string? _error;

    public StorageFieldDescriptor Descriptor { get; } = descriptor;

    public string DisplayLabel
    {
        get
        {
            var label = _uiLabelOverride ?? Descriptor.Label;
            return Descriptor.IsOptional
                ? $"{label} {Resources.OptionalField_Hint}"
                : label;
        }
    }

    public string? DisplayPlaceholder => _uiPlaceholderOverride ?? Descriptor.Placeholder;

    /// <summary>
    /// When false, the entire row (including per-option help for enums) is hidden.
    /// </summary>
    public bool IsFieldVisible => !_uiHidden;

    public string? Error
    {
        get => _error;
        protected set
        {
            if (value == _error) return;
            _error = value;
            OnPropertyChanged(nameof(Error));
        }
    }

    public void ClearError() => Error = null;

    public void SetError(string? error) => Error = error;

    internal void ResetUiCustomization()
    {
        var changed = _uiLabelOverride != null || _uiHidden || _uiPlaceholderOverride != null;
        _uiLabelOverride = null;
        _uiHidden = false;
        _uiPlaceholderOverride = null;
        if (!changed) return;
        OnPropertyChanged(nameof(DisplayLabel));
        OnPropertyChanged(nameof(IsFieldVisible));
        OnPropertyChanged(nameof(DisplayPlaceholder));
    }

    internal void ApplyUiPatch(string? labelOverride, bool? hidden, string? placeholderOverride = null)
    {
        if (labelOverride != null && _uiLabelOverride != labelOverride)
        {
            _uiLabelOverride = labelOverride;
            OnPropertyChanged(nameof(DisplayLabel));
        }

        if (hidden.HasValue && _uiHidden != hidden.Value)
        {
            _uiHidden = hidden.Value;
            OnPropertyChanged(nameof(IsFieldVisible));
        }

        if (placeholderOverride != null && _uiPlaceholderOverride != placeholderOverride)
        {
            _uiPlaceholderOverride = placeholderOverride;
            OnPropertyChanged(nameof(DisplayPlaceholder));
        }
    }

    public virtual bool Validate()
    {
        if (!IsFieldVisible || Descriptor.IsOptional)
        {
            Error = null;
            return true;
        }

        if (string.IsNullOrWhiteSpace(GetValue()))
        {
            Error = GetEmptyValidationMessage();
            return false;
        }

        Error = null;
        return true;
    }

    protected virtual string GetEmptyValidationMessage()
    {
        return Descriptor.Type switch
        {
            StorageFieldType.Folder => Resources.Field_Folder_Validation_Empty,
            StorageFieldType.Password => Resources.Password_Field_Validation_NotSpecified,
            _ => string.Format(Resources.Field_Validation_NotSpecified, Descriptor.Label),
        };
    }

    public abstract string? GetValue();
    public abstract void SetValue(string? value);
}
