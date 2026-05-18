using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.UI;
using BUtil.UI.Tasks.Controls;
using System.Threading.Tasks;

namespace BUtil.Tasks.Synchronization.UI.Controls;

public class EditSynchronizationTaskViewModel : BUtil.UI.Controls.ViewModelBase
{
    private readonly string _taskName;

    public EditSynchronizationTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;
        IsNew = isNew;

        var storeService = new TaskStore(new LocalFileSystem());
        var newTask = new TaskV2() { Model = new SynchronizationTaskModelOptionsV2() };
        if (isNew)
            newTask.Name = taskName;
        var task = isNew ? newTask : storeService.Load(taskName) ?? newTask;
        TaskIdentityViewModel = new TaskIdentityViewModel(isNew, task.Model, task.Name);
        SetWindowTitleForEdit(taskName, isNew);
        var model = (SynchronizationTaskModelOptionsV2)task.Model;
        EncryptionTaskViewModel = new BUtil.UI.Controls.EncryptionTaskViewModel(model.Password, isNew, !isNew);

        var schedule = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
        WhenTaskViewModel = new BUtil.UI.Controls.WhenTaskViewModel(isNew ? new ScheduleInfo() { Time = new System.TimeSpan(Constants.DefaultHours, Constants.DefaultMinutes, 0), Days = [System.DayOfWeek.Monday, System.DayOfWeek.Tuesday, System.DayOfWeek.Wednesday, System.DayOfWeek.Thursday, System.DayOfWeek.Friday, System.DayOfWeek.Saturday, System.DayOfWeek.Sunday] } : schedule.GetSchedule(taskName) ?? new ScheduleInfo());
        StorageViewModel = new BUtil.UI.Controls.StorageViewModel(model.To, Resources.LeftMenu_Where, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
        What = new BUtil.UI.Controls.SynchronizationWhatViewModel(model.LocalFolder, model.SynchronizationMode);
    }

    public bool IsNew { get; set; }
    public TaskIdentityViewModel TaskIdentityViewModel { get; }
    public BUtil.UI.Controls.EncryptionTaskViewModel EncryptionTaskViewModel { get; }
    public BUtil.UI.Controls.WhenTaskViewModel WhenTaskViewModel { get; }
    public BUtil.UI.Controls.StorageViewModel StorageViewModel { get; }
    public BUtil.UI.Controls.SynchronizationWhatViewModel What { get; }

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
            Model = new SynchronizationTaskModelOptionsV2
            {
                Password = EncryptionTaskViewModel.Password,
                To = StorageViewModel.GetStorageSettings(),
                LocalFolder = What.Folder,
                SynchronizationMode = What.SynchronizationMode
            }
        };

        if (!TaskV2Validator.TryValidate(newTask, true, out var error))
        {
            var detectedInfo = StorageViewModel.ApplyDetectedConnectionTrustAndBuildInfo(((SynchronizationTaskModelOptionsV2)newTask.Model).To);
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
