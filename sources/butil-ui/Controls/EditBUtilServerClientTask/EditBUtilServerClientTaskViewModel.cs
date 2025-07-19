using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using butil_ui.ViewModels;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public class EditBUtilServerClientTaskViewModel : ViewModelBase
{
    private readonly string _taskName;

    public EditBUtilServerClientTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;

        WindowTitle = isNew ? Resources.Task_Field_Name_NewDefaultValue : taskName;
        IsNew = isNew;

        var storeService = new TaskV2StoreService();
        var task = isNew ? new TaskV2() { Model = new BUtilClientModelOptionsV2(string.Empty, FileSenderDirection.ToServer, "", 999, string.Empty) } : storeService.Load(taskName) ?? new TaskV2();
        NameTaskViewModel = new NameTaskViewModel(isNew, Resources.BUtilServerClientTask_Help, task.Name);
        var model = (BUtilClientModelOptionsV2)task.Model;
        EncryptionTaskViewModel = new EncryptionTaskViewModel(model.Password, isNew, false);

        var schedule = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
        WhenTaskViewModel = new WhenTaskViewModel(isNew ? new ScheduleInfo() : schedule?.GetSchedule(taskName) ?? new ScheduleInfo());

        FolderSectionViewModel = new FolderSectionViewModel(model.Folder);
        WhereFileSenderTaskViewModel = new WhereFileSenderTaskViewModel(model.ServerHost, model.ServerPort, BUtil.Core.Localization.Resources.LeftMenu_Where, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
    }

    public bool IsNew { get; set; }
    public NameTaskViewModel NameTaskViewModel { get; }
    public EncryptionTaskViewModel EncryptionTaskViewModel { get; }
    public WhenTaskViewModel WhenTaskViewModel { get; }
    public FolderSectionViewModel FolderSectionViewModel { get; }
    public WhereFileSenderTaskViewModel WhereFileSenderTaskViewModel { get; }

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
            Model = new BUtilClientModelOptionsV2(FolderSectionViewModel.Folder, FileSenderDirection.ToServer, WhereFileSenderTaskViewModel.Host!, WhereFileSenderTaskViewModel.Port, EncryptionTaskViewModel.Password)
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