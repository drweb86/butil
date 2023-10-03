using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.ViewModels;

public class ViewModelBase : ObservableObject
{
    #region WindowTitle

    private string _windowTitle;

    public string WindowTitle
    {
        get
        {
            return _windowTitle;
        }
        set
        {
            if (value == _windowTitle)
                return;
            _windowTitle = value;
            this.OnPropertyChanged(nameof(WindowTitle));
        }
    }

    #endregion
}
