using BUtil.Core.Storages;

namespace BUtil.UI.Controls.StorageFields;

public class PasswordFieldViewModel(StorageFieldDescriptor descriptor) : StorageFieldViewModel(descriptor)
{
    private string _value = string.Empty;

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

    public override string? GetValue() => Value.Length == 0 ? null : Value;
    public override void SetValue(string? value) => Value = value ?? string.Empty;
}
