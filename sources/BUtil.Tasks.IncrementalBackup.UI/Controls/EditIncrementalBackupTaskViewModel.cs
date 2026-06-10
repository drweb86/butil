using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks;
using BUtil.Tasks.IncrementalBackup;
using BUtil.Core;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.UI;
using BUtil.UI.Tasks.Controls;
using System.Threading.Tasks;

namespace BUtil.Tasks.IncrementalBackup.UI.Controls;

public class EditIncrementalBackupTaskViewModel : BUtil.UI.Controls.ViewModelBase
{
    private readonly string _taskName;

    public EditIncrementalBackupTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;
        IsNew = isNew;

        var storeService = new TaskStore(new LocalFileSystem());
        var task = isNew
            ? new TaskV2 { Name = taskName, Model = new IncrementalBackupModelOptionsV2() }
            : storeService.Load(taskName) ?? new TaskV2 { Name = taskName, Model = new IncrementalBackupModelOptionsV2() };
        TaskIdentityViewModel = new TaskIdentityViewModel(isNew, task.Model, task.Name);
        SetWindowTitleForEdit(taskName, isNew);
        var model = (IncrementalBackupModelOptionsV2)task.Model;
        EncryptionTaskViewModel = new BUtil.UI.Controls.EncryptionTaskViewModel(model.Password, isNew, !isNew, isNew);

        var schedule = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
        WhenTaskViewModel = new BUtil.UI.Controls.WhenTaskViewModel(isNew ? new ScheduleInfo() : schedule.GetSchedule(taskName) ?? new ScheduleInfo(), isNew);
        StorageViewModel = new BUtil.UI.Controls.StorageViewModel(model.To, Resources.LeftMenu_Where, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png", isNew);
        WhatTaskViewModel = new BUtil.UI.Controls.WhatTaskViewModel(model.Items, model.FileExcludePatterns, isNew);
    }

    public bool IsNew { get; set; }
    public TaskIdentityViewModel TaskIdentityViewModel { get; }
    public BUtil.UI.Controls.EncryptionTaskViewModel EncryptionTaskViewModel { get; }
    public BUtil.UI.Controls.WhenTaskViewModel WhenTaskViewModel { get; }
    public BUtil.UI.Controls.StorageViewModel StorageViewModel { get; }
    public BUtil.UI.Controls.WhatTaskViewModel WhatTaskViewModel { get; }

    #region Commands

#pragma warning disable CA1822
    public void ButtonCancelCommand()
#pragma warning restore CA1822
    {
        TaskUINavigation.ReturnToTasksList();
    }

    public async Task ButtonOkCommand()
    {
        var newTask = new TaskV2
        {
            Name = TaskIdentityViewModel.Name.TrimEnd(),
            Model = new IncrementalBackupModelOptionsV2
            {
                Password = EncryptionTaskViewModel.Password,
                To = StorageViewModel.GetStorageSettings(),
                FileExcludePatterns = WhatTaskViewModel.GetListFileExcludePatterns(),
                Items = WhatTaskViewModel.GetListSourceItemV2s(),
            }
        };

        if (!TaskV2Validator.TryValidate(newTask, true, IsNew ? null : _taskName, out var error))
        {
            var detectedInfo = StorageViewModel.ApplyDetectedConnectionTrustAndBuildInfo(((IncrementalBackupModelOptionsV2)newTask.Model).To);
            if (!string.IsNullOrWhiteSpace(detectedInfo))
            {
                await Messages.ShowInformationBox(detectedInfo);
                return;
            }
            await Messages.ShowErrorBox(error);
            return;
        }

        var storeService = new TaskStore(new LocalFileSystem());
        var scheduler = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
        if (!IsNew)
        {
            storeService.Delete(_taskName);
            scheduler.Unschedule(_taskName);
            LogService.MoveLogs(_taskName, newTask.Name);
        }
        storeService.Save(newTask);
        scheduler.Schedule(newTask.Name, WhenTaskViewModel.GetScheduleInfo());

        TaskUINavigation.ReturnToTasksList();
    }

    #endregion

    #region Labels
    public static string Button_Cancel => Resources.Button_Cancel;
    public static string Button_OK => Resources.Button_OK;

    #endregion
}
