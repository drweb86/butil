using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Interop.Tasks;
using System;

namespace BUtil.UI.Controls;

public class LaunchTaskViewModel : ViewModelBase
{
    public LaunchTaskViewModel(string taskName, bool closeApplicationOnClose = false)
    {
        _taskName = taskName;
        _closeApplicationOnClose = closeApplicationOnClose;
        WindowTitle = taskName;
    }

    #region TaskExecuterViewModel

    private TaskExecuterViewModel? _taskExecuterViewModel;

    public TaskExecuterViewModel? TaskExecuterViewModel
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
        OnClose();
    }

#pragma warning disable CA1822 // Mark members as static
    public void StopTaskCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        PlatformSpecificExperience.Instance.SupportManager.LaunchTasksAppOrExit();
        Environment.Exit(-1);
    }

    #endregion

    #region Labels
    public static string Task_Launch_Hint => Resources.Task_Launch_Hint;
    public static string Button_Cancel => Resources.Button_Cancel;
    public static string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public static string Button_Close => Resources.Button_Close;
    public static string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;
    public static string[] PowerTasks
    {
        get => [
            Resources.AfterTaskSelection_ShutdownPc,
            Resources.AfterTaskSelection_LogOff,
            Resources.AfterTaskSelection_Reboot,
            Resources.AfterTaskSelection_DoNothing,
        ];
    }

    #endregion

    private readonly TaskEvents _taskEvents = new();
    private readonly string _taskName;
    private readonly bool _closeApplicationOnClose;
    private TaskV2? _task;

    public void Initialize()
    {
        _task = new TaskStore(new LocalFileSystem()).Load(_taskName, out var isNotFound, out var isNotSupported);
        if (isNotFound)
        {
            TaskExecuterViewModel = new TaskExecuterViewModel(string.Format(Resources.Task_Validation_NotFound, _taskName), OnClose);
            return;
        }
        if (isNotSupported)
        {
            TaskExecuterViewModel = new TaskExecuterViewModel(Resources.Task_Validation_NotSupported, OnClose);
            return;
        }

        if (_task == null)
            throw new InvalidOperationException("task is null");

        var fileLog = new FileLog(_task.Name);
        fileLog.Open();
        if (!TaskProviderRegistry.TryVerify(fileLog, _task.Model, false, out var error))
        {
            fileLog.WriteLine(LoggingEvent.Error, error);
            fileLog.Close(false);
            TaskExecuterViewModel = new TaskExecuterViewModel(error, OnClose);
            return;
        }
        fileLog.Close(true);
        System.IO.File.Delete(fileLog.LogFilename);

        TaskExecuterViewModel = new TaskExecuterViewModel(
            _taskEvents,
            _task.Name,
            (log, taskEvents, onGetLastMinuteMessage) => TaskProviderRegistry.Create(log, _task, taskEvents, onGetLastMinuteMessage),
            OnTaskCompleted,
            OnClose);
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

        var appStaysAlive = this._taskExecuterViewModel!.SelectedPowerTask == PowerTask.None;
        if (!appStaysAlive)
        {
            Environment.Exit(isOk ? 0 : -1);
        }
    }

    private void OnClose()
    {
        if (_closeApplicationOnClose)
            Environment.Exit(0);

        WindowManager.SwitchView(new TasksViewModel());
    }
}
