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

namespace butil_ui.Controls
{
    public class TaskItemViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TaskItemViewModel> _items;
        private readonly Action<PageViewModelBase> _changePage;

        public TaskItemViewModel(
            string name,
            string lastLaunchedAt,
            SolidColorBrush foreground,
            ObservableCollection<TaskItemViewModel> items,
            Action<PageViewModelBase> changePage)
        {
            SuccessForegroundColorBrush = new SolidColorBrush(ColorPalette.GetForeground(SemanticColor.Success));
            Name = name;
            LastLaunchedAt = lastLaunchedAt;
            Foreground = foreground;
            _items = items;
            _changePage = changePage;
        }

        public string Name { get; }
        public string LastLaunchedAt { get; }
        public SolidColorBrush Foreground { get; }

        public SolidColorBrush SuccessForegroundColorBrush { get; }

        public string Task_LastExecution_State => Resources.Task_LastExecution_State;
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
                _changePage(new EditIncrementalBackupTaskViewModel(task.Name, _changePage));
            else if (task.Model is ImportMediaTaskModelOptionsV2)
                _changePage(new EditMediaTaskViewModel(task.Name, _changePage));
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
