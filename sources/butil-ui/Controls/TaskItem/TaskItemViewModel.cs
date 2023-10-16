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
            SuccessForegroundColorBrush = new SolidColorBrush(ColorPalette.GetColor(SemanticColor.Success));
            Name = name;
            LastLaunchedAt = lastLaunchedAt;
            Foreground = ColorPalette.GetResultColor(status);
            ForegroundWindowFontAccented = new SolidColorBrush(ColorPalette.GetColor(SemanticColor.ForegroundWindowFontAccented));
            switch (status)
            {
                case ProcessingStatus.NotStarted: TaskState = string.Empty; break;
                case ProcessingStatus.FinishedWithErrors: TaskState = "⛔"; break;
                case ProcessingStatus.FinishedSuccesfully: TaskState = "✅"; break;
                case ProcessingStatus.InProgress: TaskState = " ?"; break;
                default: throw new ArgumentOutOfRangeException(nameof(status));
            }
            _items = items;
        }

        public string Name { get; }
        public string LastLaunchedAt { get; }
        public SolidColorBrush Foreground { get; }

        public SolidColorBrush SuccessForegroundColorBrush { get; }
        public SolidColorBrush ForegroundWindowFontAccented { get; }

        public string TaskState { get; }
        public string Task_Delete_Hint => Resources.Task_Delete_Hint;
        public string Task_Launch_Hint => Resources.Task_Launch_Hint;
        public string Task_Edit_Hint => Resources.Task_Edit_Hint;
        public string Task_Restore => Resources.Task_Restore;

        #region Commands

        public void TaskLaunchCommand()
        {
            SupportManager.LaunchTask(Name);
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
            SupportManager.OpenRestorationApp(Name);
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
