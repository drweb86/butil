using BUtil.Core.Storages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BUtil.UI.Controls.StorageFields;

public class EnumFieldViewModel : StorageFieldViewModel
{
    private readonly IReadOnlyList<(string Value, string DisplayLabel)> _options;
    private string _selectedDisplay;

    public EnumFieldViewModel(StorageFieldDescriptor descriptor) : base(descriptor)
    {
        _options = descriptor.Options ?? [];
        DisplayOptions = new ObservableCollection<string>(_options.Select(o => o.DisplayLabel));

        var defaultVal = descriptor.DefaultValue as string;
        var match = _options.FirstOrDefault(o => o.Value == defaultVal);
        _selectedDisplay = match.DisplayLabel ?? (_options.Count > 0 ? _options[0].DisplayLabel : string.Empty);
    }

    public ObservableCollection<string> DisplayOptions { get; }

    public string SelectedDisplay
    {
        get => _selectedDisplay;
        set
        {
            if (value == _selectedDisplay) return;
            _selectedDisplay = value;
            OnPropertyChanged(nameof(SelectedDisplay));
        }
    }

    // Returns the serialized Value key, not the display label
    public override string? GetValue()
    {
        var match = _options.FirstOrDefault(o => o.DisplayLabel == _selectedDisplay);
        return match.Value ?? (_options.Count > 0 ? _options[0].Value : null);
    }

    public override void SetValue(string? value)
    {
        var match = _options.FirstOrDefault(o => o.Value == value);
        SelectedDisplay = match.DisplayLabel ?? (_options.Count > 0 ? _options[0].DisplayLabel : string.Empty);
    }
}
