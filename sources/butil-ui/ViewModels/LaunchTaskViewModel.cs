using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.TasksTree;
using butil_ui.Controls;
using System;

namespace butil_ui.ViewModels;

public class LaunchTaskViewModel : ViewModelBase
{
    public LaunchTaskViewModel(string taskName)
    {
        _taskName = taskName;
        WindowTitle = taskName;
    }

    #region TaskExecuterViewModel

    private TaskExecuterViewModel _taskExecuterViewModel;

    public TaskExecuterViewModel TaskExecuterViewModel
    {
        get
        {
            return _taskExecuterViewModel;
        }
        set
        {
            if (value == _taskExecuterViewModel)
                return;
            _taskExecuterViewModel = value;
            OnPropertyChanged(nameof(TaskExecuterViewModel));
        }
    }

    #endregion

    #region DarkThemeLabel

    private string _darkThemeLabel = string.Empty;
    public string DarkThemeLabel
    {
        get
        {
            return _darkThemeLabel;
        }
        set
        {
            if (value == _darkThemeLabel)
                return;
            _darkThemeLabel = value;
            OnPropertyChanged(nameof(DarkThemeLabel));
        }
    }

    #endregion

    #region LightThemeLabel

    private string _lightThemeLabel = string.Empty;
    public string LightThemeLabel
    {
        get
        {
            return _lightThemeLabel;
        }
        set
        {
            if (value == _lightThemeLabel)
                return;
            _lightThemeLabel = value;
            OnPropertyChanged(nameof(LightThemeLabel));
        }
    }

    #endregion

    #region Commands

    public void CloseCommand()
    {
        Environment.Exit(0);
    }

    public void StopTaskCommand()
    {
        Environment.Exit(-1);
    }

    #endregion

    #region Labels
    public string Theme_Title => "🎨 " + BUtil.Core.Localization.Resources.Theme_Title;
    public string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public string Button_Cancel => Resources.Button_Cancel;
    public string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public string Button_Close => Resources.Button_Close;
    public string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;
    public string[] PowerTasks
    {
        get => new[] {
            Resources.AfterTaskSelection_ShutdownPc,
            Resources.AfterTaskSelection_LogOff,
            Resources.AfterTaskSelection_Reboot,
            Resources.AfterTaskSelection_DoNothing,
        };
    }

    #endregion

    private readonly TaskEvents _taskEvents = new();
    private readonly string _taskName;
    private TaskV2? _task;

    public void Initialize()
    {
        _task = new TaskV2StoreService().Load(_taskName, out var isNotFound, out var isNotSupported);
        if (isNotFound)
        {
            TaskExecuterViewModel = new TaskExecuterViewModel(string.Format(Resources.Task_Validation_NotFound, _taskName));
            return;
        }
        if (isNotSupported)
        {
            TaskExecuterViewModel = new TaskExecuterViewModel(Resources.Task_Validation_NotSupported);
            return;
        }

        if (!RootTaskFactory.TryVerify(new StubLog(), _task.Model, out var error))
        {
            TaskExecuterViewModel = new TaskExecuterViewModel(error);
            return;
        }

        TaskExecuterViewModel = new TaskExecuterViewModel(
            _taskEvents,
            _task.Name,
            (log, taskEvents) => RootTaskFactory.Create(log, _task, taskEvents),
            OnTaskCompleted);
    }


    private void OnTaskCompleted(bool isOk)
    {
        if (!isOk)
        {
            WindowTitle = $"{_taskName} - {Resources.Task_Status_FailedSeeLog}";
        }
        else
        {
            WindowTitle = $"{_taskName} - {Resources.Task_Status_Succesfull}";
        }

        var appStaysAlive = this._taskExecuterViewModel.SelectedPowerTask == PowerTask.None;
        if (!appStaysAlive)
        {
            Environment.Exit(isOk ? 0 : -1);
        }
    }
}
