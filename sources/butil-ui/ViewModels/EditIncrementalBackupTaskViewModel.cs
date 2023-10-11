using Avalonia.Media;
using Avalonia.Threading;
using BUtil.Core;
using BUtil.Core.BackupModels;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.TasksTree.Core;
using butil_ui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace butil_ui.ViewModels;

public class EditIncrementalBackupTaskViewModel : PageViewModelBase
{
    private readonly string _taskName;

    public EditIncrementalBackupTaskViewModel(string taskName)
    {
        _taskName = string.IsNullOrEmpty(taskName) ? Resources.Task_Field_Name_NewDefaultValue : taskName; ;

        WindowTitle = taskName;
    }

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

    #region Labels
    public string Button_Cancel => Resources.Button_Cancel;
    public string Button_OK => Resources.Button_OK;

    #endregion


    public void Initialize()
    {
    }
}
