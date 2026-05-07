using BUtil.Core.Localization;
using BUtil.Core.Storages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BUtil.UI.Controls.StorageFields;

public abstract class StorageFieldViewModel(StorageFieldDescriptor descriptor) : ObservableObject
{
    public StorageFieldDescriptor Descriptor { get; } = descriptor;

    public string DisplayLabel => Descriptor.IsOptional
        ? $"{Descriptor.Label} {Resources.OptionalField_Hint}"
        : Descriptor.Label;

    public abstract string? GetValue();
    public abstract void SetValue(string? value);
}
