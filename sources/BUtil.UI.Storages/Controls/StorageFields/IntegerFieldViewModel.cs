using BUtil.Core.Storages;

namespace BUtil.UI.Controls.StorageFields;

public class IntegerFieldViewModel : StorageFieldViewModel
{
    private readonly decimal _min;
    private readonly decimal _max;
    private readonly long _defaultLong;
    private decimal _value;

    public IntegerFieldViewModel(StorageFieldDescriptor descriptor) : base(descriptor)
    {
        _min = (decimal)(descriptor.Min ?? 0);
        _max = (decimal)(descriptor.Max ?? 65535);
        _defaultLong = descriptor.DefaultValue is long d ? d : 0L;
        _value = (decimal)_defaultLong;
    }

    public decimal Value
    {
        get => _value;
        set
        {
            if (value == _value) return;
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public decimal Minimum => _min;
    public decimal Maximum => _max;

    public override string? GetValue() => ((long)_value).ToString();
    public override void SetValue(string? value) => Value = long.TryParse(value, out var v) ? v : _defaultLong;
}
