using BUtil.Core.Storages;

namespace BUtil.UI.Controls.StorageFields;

public class TextFieldViewModel(StorageFieldDescriptor descriptor) : StorageFieldViewModel(descriptor)
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

    public override string? GetValue() => Value;
    public override void SetValue(string? value) => Value = value ?? string.Empty;
}
