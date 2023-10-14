using BUtil.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.ViewModels;

public class ViewModelBase : ObservableObject
{
    public bool IsFullMenuVisible { get; set; }

    #region WindowTitle

    private string _windowTitle = "BUtil - V" + CopyrightInfo.Version.ToString(3);

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
