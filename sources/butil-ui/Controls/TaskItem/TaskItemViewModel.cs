using Avalonia.Media;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Options;
using butil_ui.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public class TaskItemViewModel(
    string name,
    string lastLaunchedAt,
    ProcessingStatus status,
    ObservableCollection<TaskItemViewModel> items) : ObservableObject
{
    private readonly ObservableCollection<TaskItemViewModel> _items = items;

    public string Name { get; } = name;
    public string LastLaunchedAt { get; } = lastLaunchedAt;
    public SolidColorBrush Foreground { get; } = ColorPalette.GetProcessingStatusBrush(status);
    public SolidColorBrush Background { get; } = ColorPalette.GetBrush(SemanticColor.WindowFrontBackground);

    public SolidColorBrush SuccessForegroundColorBrush { get; } = ColorPalette.GetBrush(SemanticColor.Success);
    public SolidColorBrush ForegroundWindowFontAccented { get; } = ColorPalette.GetBrush(SemanticColor.ForegroundWindowFontAccented);

    public static string Task_Delete => Resources.Task_Delete;
    public static string Task_Launch => Resources.Task_Launch;
    public static string Task_Edit => Resources.Task_Edit;
    public static string Task_Restore => Resources.Task_Restore;

    #region Commands

    public void TaskLaunchCommand()
    {
        PlatformSpecificExperience.Instance.SupportManager
            .LaunchTask(Name);
    }

    public void TaskEditCommand()
    {
        var task = new TaskV2StoreService()
            .Load(this.Name);
        if (task == null)
            return;

        if (task.Model is IncrementalBackupModelOptionsV2)
            WindowManager.SwitchView(new EditIncrementalBackupTaskViewModel(task.Name, false));
        else if (task.Model is SynchronizationTaskModelOptionsV2)
            WindowManager.SwitchView(new EditSynchronizationTaskViewModel(task.Name, false));
        else if (task.Model is ImportMediaTaskModelOptionsV2)
            WindowManager.SwitchView(new EditMediaTaskViewModel(task.Name, false));
    }

    public void TaskRestoreCommand()
    {
        PlatformSpecificExperience.Instance.SupportManager
            .OpenRestorationApp(Name);
    }

    public async Task TaskDeleteCommand()
    {
        if (!await Messages.ShowYesNoDialog(string.Format(Resources.Task_Delete_Confirm, Name)))
            return;

        new TaskV2StoreService()
            .Delete(Name);
        PlatformSpecificExperience.Instance
            .GetTaskSchedulerService()?.Unschedule(Name);
        _items.Remove(this);
    }

    #endregion
}
