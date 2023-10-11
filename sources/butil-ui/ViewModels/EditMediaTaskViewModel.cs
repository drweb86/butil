using Avalonia.Media;
using BUtil.Core.Localization;
using System;

namespace butil_ui.ViewModels;

public class EditMediaTaskViewModel : PageViewModelBase
{
    public EditMediaTaskViewModel(string taskName)
    {
        _taskName = string.IsNullOrEmpty(taskName) ? Resources.Task_Field_Name_NewDefaultValue : taskName;
        WindowTitle = _taskName;
    }

    #region Labels
    public string Button_Cancel => Resources.Button_Cancel;
    public string Button_OK => Resources.Button_OK;
    #endregion

    #region Commands

    public void ButtonCancelCommand()
    {
        WindowManager.SwitchView(new TasksViewModel());
    }

    public void ButtonOkCommand()
    {
        // TODO: actual saving

        WindowManager.SwitchView(new TasksViewModel());
    }

    #endregion

    private readonly string _taskName;

    public void Initialize()
    {
    }
}
