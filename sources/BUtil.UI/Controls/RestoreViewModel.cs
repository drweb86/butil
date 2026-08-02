using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks.Events;
using BUtil.Core.Localization;
using BUtil.Tasks.Common.States;
using System;
using System.Collections.Generic;

namespace BUtil.UI.Controls;

public class RestoreViewModel : ViewModelBase
{
    public RestoreViewModel(IStorageSettingsV2? storageSettingsV2, string? password)
    {
        WindowTitle = Resources.Task_Restore;

        StorageViewModel = new StorageViewModel(storageSettingsV2 ?? new FolderStorageSettingsV2(), Resources.Task_Restore, help: Resources.Task_Restore_Storage_Help);
        StorageViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(StorageViewModel.SelectedProvider)
                or nameof(StorageViewModel.Quota)
                or nameof(StorageViewModel.Error))
                FormErrorsText = null;
        };
        EncryptionTaskViewModel = new EncryptionTaskViewModel(password ?? string.Empty, false);
        EncryptionTaskViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EncryptionTaskViewModel.Password))
                FormErrorsText = null;
        };
    }

    #region FormErrorsText

    private string? _formErrorsText;

    public string? FormErrorsText
    {
        get => _formErrorsText;
        set
        {
            if (value == _formErrorsText)
                return;
            _formErrorsText = value;
            OnPropertyChanged(nameof(FormErrorsText));
        }
    }

    #endregion

    #region TaskExecuterViewModel

    private TaskExecuterViewModel? _taskExecuterViewModel;

    public TaskExecuterViewModel? TaskExecuterViewModel
    {
        get => _taskExecuterViewModel;
        set
        {
            if (value == _taskExecuterViewModel)
                return;
            _taskExecuterViewModel = value;
            OnPropertyChanged(nameof(TaskExecuterViewModel));
        }
    }

    #endregion

    public StorageViewModel StorageViewModel { get; }
    public EncryptionTaskViewModel EncryptionTaskViewModel { get; }

    #region Commands

#pragma warning disable CA1822 // Mark members as static
    public void CloseCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TasksViewModel());
    }

    public void ContinueCommand()
    {
        FormErrorsText = null;
        var errors = new List<string>();
        if (!StorageViewModel.Validate())
            errors.Add($"{StorageViewModel.Title}: {StorageViewModel.Error}");
        if (!EncryptionTaskViewModel.Validate())
            errors.Add($"{Resources.LeftMenu_Encryption}: {EncryptionTaskViewModel.PasswordError}");
        if (errors.Count > 0)
        {
            FormErrorsText = string.Join(Environment.NewLine, errors);
            return;
        }

        var storageOptions = StorageViewModel.GetStorageSettings();

        var taskEvents = new TaskEvents();
        GetExistingVersionStateFromStorageRootTask openIncrementalBackupTask = null!;
        this.TaskExecuterViewModel = new TaskExecuterViewModel(
            taskEvents,
            Resources.Task_Restore,
            (log, taskEvents, getLastMinuteMessage) => 
            {
                openIncrementalBackupTask = new GetExistingVersionStateFromStorageRootTask(log, taskEvents, storageOptions, EncryptionTaskViewModel.Password, getLastMinuteMessage);
                return openIncrementalBackupTask;
            },
            isOk =>
            {
                if (isOk)
                {
                    WindowManager.SwitchView(new RestoreVersionsViewModel(
                        openIncrementalBackupTask.StorageState!,
                        storageOptions,
                        EncryptionTaskViewModel.Password));
                }
            });

        TaskExecuterViewModel.StartTaskCommand();
    }

    #endregion

    #region Labels

    public static string AfterTaskSelection_Field => Resources.AfterTaskSelection_Field;
    public static string Button_Close => Resources.Button_Close;
    public static string AfterTaskSelection_Help => Resources.AfterTaskSelection_Help;
    public static string Button_Continue => Resources.Button_Continue;
    public static string Task_Restore => Resources.Task_Restore;

    #endregion
}
