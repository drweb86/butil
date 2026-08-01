using BUtil.Core.Localization;
using BUtil.Core.Storages;
using System.Windows.Input;

namespace BUtil.UI.Controls.StorageFields;

public class FileFieldViewModel(StorageFieldDescriptor descriptor) : StorageFieldViewModel(descriptor)
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
            Error = null;
        }
    }

    public static string BrowseLabel => Resources.Field_File_Browse;

    public ICommand? BrowseCommand { get; set; }

    public override string? GetValue() => Value.Length == 0 ? null : Value;
    public override void SetValue(string? value) => Value = value ?? string.Empty;
}
