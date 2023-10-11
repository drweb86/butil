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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace butil_ui.ViewModels;

public class EditIncrementalBackupTaskViewModel : PageViewModelBase
{
    private readonly string _taskName;
    private readonly Action<PageViewModelBase> _changePage;

    public EditIncrementalBackupTaskViewModel(string taskName, Action<PageViewModelBase> changePage)
    {
        _taskName = taskName;
        _changePage = changePage;

        WindowTitle = taskName;
    }

    #region Commands


    #endregion

    #region Labels
    public string Button_Cancel => Resources.Button_Cancel;
    public string Button_OK => Resources.Button_OK;

    #endregion

    public void Initialize()
    {
    }
}
