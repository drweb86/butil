using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using butil_ui.ViewModels;
using System.Threading.Tasks;

namespace butil_ui.Controls;


public class EditSynchronizationTaskViewModel : ViewModelBase
{
    private readonly string _taskName;

    public EditSynchronizationTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;

        WindowTitle = isNew ? Resources.Task_Field_Name_NewDefaultValue : taskName;
        IsNew = isNew;

        var storeService = new TaskV2StoreService();
        var newTask = new TaskV2() { Model = new SynchronizationTaskModelOptionsV2() };
        var task = isNew ? newTask : storeService.Load(taskName) ?? newTask;
        NameTaskViewModel = new NameTaskViewModel(isNew, Resources.SynchronizationTask_Help, task.Name);
        var model = (SynchronizationTaskModelOptionsV2)task.Model;
        EncryptionTaskViewModel = new EncryptionTaskViewModel(model.Password, isNew, !isNew);

        var schedule = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
        WhenTaskViewModel = new WhenTaskViewModel(isNew ? new ScheduleInfo() { Time = new System.TimeSpan(Constants.DefaultHours, Constants.DefaultMinutes, 0), Days = [System.DayOfWeek.Monday, System.DayOfWeek.Tuesday, System.DayOfWeek.Wednesday, System.DayOfWeek.Thursday, System.DayOfWeek.Friday, System.DayOfWeek.Saturday, System.DayOfWeek.Sunday] } : schedule?.GetSchedule(taskName) ?? new ScheduleInfo());
        WhereTaskViewModel = new WhereTaskViewModel(model.To, Resources.LeftMenu_Where, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
        What = new SynchronizationWhatViewModel(model.LocalFolder, model.RepositorySubfolder, model.SynchronizationMode);
    }

    public bool IsNew { get; set; }
    public NameTaskViewModel NameTaskViewModel { get; }
    public EncryptionTaskViewModel EncryptionTaskViewModel { get; }
    public WhenTaskViewModel WhenTaskViewModel { get; }
    public WhereTaskViewModel WhereTaskViewModel { get; }
    public SynchronizationWhatViewModel What { get; }

    #region Commands

#pragma warning disable CA1822 // Mark members as static
    public void ButtonCancelCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TasksViewModel());
    }

    public async Task ButtonOkCommand()
    {
        var newTask = new TaskV2
        {
            Name = NameTaskViewModel.Name,
            Model = new SynchronizationTaskModelOptionsV2
            {
                Password = EncryptionTaskViewModel.Password,
                To = WhereTaskViewModel.GetStorageSettings(),
                LocalFolder = What.Folder,
                RepositorySubfolder = string.IsNullOrWhiteSpace(What.RepositorySubfolder) ? null : What.RepositorySubfolder,
                SynchronizationMode = What.SynchronizationMode
            }
        };

        if (!TaskV2Validator.TryValidate(newTask, true, out var error))
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
            LogService.MoveLogs(_taskName, newTask.Name);
        }
        storeService.Save(newTask);
        scheduler?.Schedule(newTask.Name, WhenTaskViewModel.GetScheduleInfo());

        WindowManager.SwitchView(new TasksViewModel());
    }

    #endregion

    #region Labels
    public static string Button_Cancel => Resources.Button_Cancel;
    public static string Button_OK => Resources.Button_OK;

    #endregion
}