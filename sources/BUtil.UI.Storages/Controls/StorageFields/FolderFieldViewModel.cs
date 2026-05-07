using BUtil.Core.Localization;
using BUtil.Core.Storages;

namespace BUtil.UI.Controls.StorageFields;

public class FolderFieldViewModel(StorageFieldDescriptor descriptor) : StorageFieldViewModel(descriptor)
{
    private string _value = descriptor.DefaultValue as string ?? string.Empty;

    public string Value
    {
        get => _value;
        set
        {
            if (value == _value) return;
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public static string BrowseLabel => Resources.Field_Folder_Browse;

    public override string? GetValue() => Value;
    public override void SetValue(string? value) => Value = value ?? string.Empty;
}
