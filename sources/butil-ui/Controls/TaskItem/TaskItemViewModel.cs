using Avalonia.Media;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using butil_ui.ViewModels;
using System.Threading.Tasks;
using BUtil.Core.Options;
using BUtil.Core;
using System.Collections.ObjectModel;
using System;
using BUtil.Core.State;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls
{
    public class TaskItemViewModel : ObservableObject
    {
        private readonly ObservableCollection<TaskItemViewModel> _items;

        public TaskItemViewModel(
            string name,
            string lastLaunchedAt,
            ProcessingStatus status,
            ObservableCollection<TaskItemViewModel> items)
        {
            SuccessForegroundColorBrush = ColorPalette.GetBrush(SemanticColor.Success);
            Name = name;
            LastLaunchedAt = lastLaunchedAt;
            Background = ColorPalette.GetBrush(SemanticColor.WindowFrontBackground);
            Foreground = ColorPalette.GetProcessingStatusBrush(status);
            ForegroundWindowFontAccented = ColorPalette.GetBrush(SemanticColor.ForegroundWindowFontAccented);
            _items = items;
        }

        public string Name { get; }
        public string LastLaunchedAt { get; }
        public SolidColorBrush Foreground { get; }
        public SolidColorBrush Background { get; }

        public SolidColorBrush SuccessForegroundColorBrush { get; }
        public SolidColorBrush ForegroundWindowFontAccented { get; }

        public string Task_Delete => Resources.Task_Delete;
        public string Task_Launch => Resources.Task_Launch;
        public string Task_Edit => Resources.Task_Edit;
        public string Task_Restore => Resources.Task_Restore;

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
}
