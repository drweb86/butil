using BUtil.Core.Storages;

namespace BUtil.UI.Controls.StorageFields;

public class IntegerFieldViewModel : StorageFieldViewModel
{
    private readonly long _min;
    private readonly long _max;
    private readonly long _defaultLong;
    private long _value;

    public IntegerFieldViewModel(StorageFieldDescriptor descriptor) : base(descriptor)
    {
        _min = (long)(descriptor.Min ?? 0);
        _max = (long)(descriptor.Max ?? 65535);
        _defaultLong = descriptor.DefaultValue is long d ? d : 0L;
        _value = _defaultLong;
    }

    public long Value
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

    public long Minimum => _min;
    public long Maximum => _max;

    public override bool Validate()
    {
        if (!IsFieldVisible || Descriptor.IsOptional)
        {
            Error = null;
            return true;
        }

        if (_value < _min || _value > _max)
        {
            Error = GetEmptyValidationMessage();
            return false;
        }

        Error = null;
        return true;
    }

    public override string? GetValue() => _value.ToString();
    public override void SetValue(string? value) => Value = long.TryParse(value, out var v) ? v : _defaultLong;
}
