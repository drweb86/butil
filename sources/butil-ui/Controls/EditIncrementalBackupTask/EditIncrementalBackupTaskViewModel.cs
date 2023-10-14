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
using System;
using System.Linq;
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

            var storeService = new TaskV2StoreService();
            var task = isNew ? new TaskV2() : storeService.Load(taskName) ?? new TaskV2();
            NameTaskViewModel = new NameTaskViewModel(isNew, Resources.IncrementalBackup_Help, task.Name);
            var model = (IncrementalBackupModelOptionsV2)task.Model;
            EncryptionTaskViewModel = new EncryptionTaskViewModel(model.Password);

            var schedule = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
            WhenTaskViewModel = new WhenTaskViewModel(isNew ? new ScheduleInfo() : schedule?.GetSchedule(taskName) ?? new ScheduleInfo());
            WhereTaskViewModel = new WhereTaskViewModel(model.To, Resources.LeftMenu_Where, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
            WhatTaskViewModel = new WhatTaskViewModel(model.Items, model.FileExcludePatterns);
        }

        public bool IsNew { get; set; }
        public NameTaskViewModel NameTaskViewModel { get; }
        public EncryptionTaskViewModel EncryptionTaskViewModel { get; }
        public WhenTaskViewModel WhenTaskViewModel { get; }
        public WhereTaskViewModel WhereTaskViewModel { get; }
        public WhatTaskViewModel WhatTaskViewModel { get; }

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
                Model = new IncrementalBackupModelOptionsV2
                {
                    Password = EncryptionTaskViewModel.Password,
                    To = WhereTaskViewModel.GetStorageSettings(),
                    FileExcludePatterns = WhatTaskViewModel.GetListFileExcludePatterns(),
                    Items = WhatTaskViewModel.GetListSourceItemV2s(),
                }
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
            scheduler?.Schedule(newTask.Name, WhenTaskViewModel.GetScheduleInfo());

            WindowManager.SwitchView(new TasksViewModel());
        }

        #endregion

        #region Labels
        public string Button_Cancel => Resources.Button_Cancel;
        public string Button_OK => Resources.Button_OK;

        #endregion
    }
}