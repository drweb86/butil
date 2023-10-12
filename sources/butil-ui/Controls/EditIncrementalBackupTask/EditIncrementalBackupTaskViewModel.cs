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
using butil_ui.ViewModels;
using System.Threading.Tasks;

namespace butil_ui.Controls
{

    public class EditIncrementalBackupTaskViewModel : PageViewModelBase
    {
        private readonly string _taskName;

        public EditIncrementalBackupTaskViewModel(string taskName, bool isNew)
        {
            _taskName = taskName;

            WindowTitle = isNew ? Resources.Task_Field_Name_NewDefaultValue : taskName;
            IsNew = isNew;
            NameTaskViewModel = new NameTaskViewModel(isNew, Resources.IncrementalBackup_Help, isNew ? Resources.Task_Field_Name_NewDefaultValue : taskName);
        }

        public bool IsNew { get; set; }
        public NameTaskViewModel NameTaskViewModel { get; }

        #region Commands

        public void ButtonCancelCommand()
        {
            WindowManager.SwitchView(new TasksViewModel());
        }

        public async Task ButtonOkCommand()
        {
            var newTask = new TaskV2
            {
                Name = NameTaskViewModel.Name,
            };

            if (!TaskV2Validator.TryValidate(newTask, out var error))
            {
                await Messages.ShowErrorBox(error);
                return;
            }

            var storeService = new TaskV2StoreService();
            var scheduler = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
            if (!IsNew)
            {
                storeService.Delete(_taskName);
                scheduler?.Unschedule(_taskName);
            }
            storeService.Save(newTask);
            // TODO: scheduler?.Schedule(_taskName);

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
}