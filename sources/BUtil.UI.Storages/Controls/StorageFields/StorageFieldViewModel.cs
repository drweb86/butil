using BUtil.Core.Localization;
using BUtil.Core.Storages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BUtil.UI.Controls.StorageFields;

public abstract class StorageFieldViewModel(StorageFieldDescriptor descriptor) : ObservableObject
{
    private string? _uiLabelOverride;
    private bool _uiHidden;

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

    /// <summary>
    /// When false, the entire row (including per-option help for enums) is hidden.
    /// </summary>
    public bool IsFieldVisible => !_uiHidden;

    internal void ResetUiCustomization()
    {
        var changed = _uiLabelOverride != null || _uiHidden;
        _uiLabelOverride = null;
        _uiHidden = false;
        if (!changed) return;
        OnPropertyChanged(nameof(DisplayLabel));
        OnPropertyChanged(nameof(IsFieldVisible));
    }

    internal void ApplyUiPatch(string? labelOverride, bool? hidden)
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
    }

    public abstract string? GetValue();
    public abstract void SetValue(string? value);
}
