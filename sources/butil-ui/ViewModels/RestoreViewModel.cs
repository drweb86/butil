using Avalonia.Threading;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.States;
using butil_ui.Controls;
using System;
using System.Threading.Tasks;

namespace butil_ui.ViewModels;

public class RestoreViewModel : ViewModelBase
{
    public RestoreViewModel(IStorageSettingsV2? storageSettingsV2, string? password)
    {
        WindowTitle = Resources.Task_Restore;

        WhereTaskViewModel = new WhereTaskViewModel(storageSettingsV2 ?? new FolderStorageSettingsV2(), Resources.Task_Restore, "/Assets/CrystalClear_EveraldoCoelho_Storages48x48.png");
        EncryptionTaskViewModel = new EncryptionTaskViewModel(password ?? string.Empty, false);
        VersionsListViewModel = new VersionsListViewModel(this);
    }

    #region IsSetupVisible

    private bool _isSetupVisible = true;

    public bool IsSetupVisible
    {
        get
        {
            return _isSetupVisible;
        }
        set
        {
            if (value == _isSetupVisible)
                return;
            _isSetupVisible = value;
            OnPropertyChanged(nameof(IsSetupVisible));
        }
    }

    #endregion

    #region TaskExecuterViewModel

    private TaskExecuterViewModel? _taskExecuterViewModel;

    public TaskExecuterViewModel? TaskExecuterViewModel
    {
        get
        {
            return _taskExecuterViewModel;
        }
        set
        {
            if (value == _taskExecuterViewModel)
                return;
            _taskExecuterViewModel = value;
            OnPropertyChanged(nameof(TaskExecuterViewModel));
        }
    }

    #endregion

    public WhereTaskViewModel WhereTaskViewModel { get; }
    public EncryptionTaskViewModel EncryptionTaskViewModel { get; }

    public VersionsListViewModel VersionsListViewModel { get; }

    #region Commands

    public void CloseCommand()
    {
        Environment.Exit(0);
    }

    public async Task ContinueCommand()
    {
        var storageOptions = WhereTaskViewModel.GetStorageSettings();
        if (storageOptions == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(EncryptionTaskViewModel.Password))
        {
            await Messages.ShowErrorBox(Resources.Password_Field_Validation_NotSpecified);
            return;
        }

        var taskEvents = new TaskEvents();
        GetExistingVersionStateFromStorageTask openIncrementalBackupTask = null;
        this.TaskExecuterViewModel = new TaskExecuterViewModel(
            taskEvents,
            Resources.Task_Restore,
            log => 
            {
                openIncrementalBackupTask = new GetExistingVersionStateFromStorageTask(log, taskEvents, storageOptions, EncryptionTaskViewModel.Password);
                return openIncrementalBackupTask;
            },
            isOk =>
            {
                if (isOk)
                {
                    IsSetupVisible = false;
                    TaskExecuterViewModel = null!;
                    VersionsListViewModel.Initialize(openIncrementalBackupTask.StorageState!, storageOptions, EncryptionTaskViewModel.Password);
                }
            });

        TaskExecuterViewModel.StartTaskCommand();
    }

    #endregion

    #region Labels

    public string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public string Button_Close => Resources.Button_Close;
    public string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;
    public string Button_Continue => Resources.Button_Continue;
    public string Task_Restore => Resources.Task_Restore;

    #endregion
}
