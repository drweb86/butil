using System.ComponentModel;
using BUtil.Core;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BUtil.UI.Controls;

public class ViewModelBase : ObservableObject
{
    public bool IsFullMenuVisible { get; set; }

    #region SearchText

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (value == _searchText)
                return;
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }

    #endregion

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

    protected void SetWindowTitleForEdit(string name, bool isNew)
    {
        var suffix = isNew ? Resources.Task_WindowTitle_Creating : Resources.Task_WindowTitle_Editing;
        WindowTitle = $"{name} - {suffix}";
    }

    #endregion
}
