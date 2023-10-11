using BUtil.Core.Localization;
using System;

namespace butil_ui.ViewModels;

public class EditMediaTaskViewModel : PageViewModelBase
{
    public EditMediaTaskViewModel(string taskName, Action<PageViewModelBase> changePage)
    {
        _taskName = taskName;
        _changePage = changePage;
        WindowTitle = taskName;
    }

    #region Labels
    public string Button_Cancel => Resources.Button_Cancel;
    public string Button_OK => Resources.Button_OK;
    #endregion

    private readonly string _taskName;
    private readonly Action<PageViewModelBase> _changePage;

    public void Initialize()
    {
    }
}
