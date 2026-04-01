using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.State;
using butil_ui.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public class TaskCardViewModel(
    string name,
    DateTime? lastLaunchedAt,
    ProcessingStatus status,
    ObservableCollection<TaskCardViewModel> items,
    string? logFilePath) : ObservableObject
{
    private readonly ObservableCollection<TaskCardViewModel> _items = items;
    private readonly string? _logFilePath = logFilePath;

    public string Name { get; } = name;
    public string OpenLogTooltip { get; } = Resources.Task_OpenLog + (lastLaunchedAt.HasValue ? ("\n" + lastLaunchedAt) : string.Empty);
    public string? LastLaunchedAt { get; } = lastLaunchedAt.HasValue ? lastLaunchedAt.Value.ToShortDateString() : null;
    public SolidColorBrush Foreground { get; } = ColorPalette.GetProcessingStatusBrush(status);
    public SolidColorBrush Background { get; } = ColorPalette.GetBrush(SemanticColor.WindowFrontBackground);

    public SolidColorBrush SuccessForegroundColorBrush { get; } = ColorPalette.GetBrush(SemanticColor.Success);
    public SolidColorBrush ForegroundWindowFontAccented { get; } = ColorPalette.GetBrush(SemanticColor.ForegroundWindowFontAccented);

    public bool HasLog => _logFilePath != null;

    public static string Task_Delete => Resources.Task_Delete;
    public static string Task_Launch => Resources.Task_Launch;
    public static string Task_Edit => Resources.Task_Edit;
    public static string Task_Restore => Resources.Task_Restore;
    public static string Task_OpenLog => Resources.Task_OpenLog;

    #region Commands

    public void TaskLaunchCommand()
    {
        WindowManager.SwitchToLaunchTask(Name);
    }

    public void OpenLogCommand()
    {
        if (_logFilePath != null)
            ProcessHelper.ShellExecute(_logFilePath);
    }

    public void TaskEditCommand()
    {
        var task = new TaskV2StoreService(new LocalFileSystem())
            .Load(this.Name);
        if (task == null)
            return;

        if (task.Model is IncrementalBackupModelOptionsV2)
            WindowManager.SwitchView(new EditIncrementalBackupTaskViewModel(task.Name, false));
        else if (task.Model is SynchronizationTaskModelOptionsV2)
            WindowManager.SwitchView(new EditSynchronizationTaskViewModel(task.Name, false));
        else if (task.Model is ImportMediaTaskModelOptionsV2)
            WindowManager.SwitchView(new EditMediaTaskViewModel(task.Name, false));
        else if (task.Model is BUtilServerModelOptionsV2)
            WindowManager.SwitchView(new EditBUtilServerTaskViewModel(task.Name, false));
        else if (task.Model is BUtilClientModelOptionsV2)
            WindowManager.SwitchView(new EditBUtilServerClientTaskViewModel(task.Name, false));
    }

    public void TaskRestoreCommand()
    {
        WindowManager.SwitchToRestorationView(Name);
    }

    public async Task TaskDeleteCommand()
    {
        if (!await Messages.ShowYesNoDialog(string.Format(Resources.Task_Delete_Confirm, Name)))
            return;

        new TaskV2StoreService(new LocalFileSystem())
            .Delete(Name);
        LogService.DeleteLogs(Name);
        ImportMediaFileService.DeleteState(Name);
        PlatformSpecificExperience.Instance.GetTaskSchedulerService().Unschedule(Name);
        _items.Remove(this);
    }

    #endregion
}
