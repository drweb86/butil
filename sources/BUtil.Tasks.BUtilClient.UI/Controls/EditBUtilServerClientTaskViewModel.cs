using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Tasks.BUtilClient;
using BUtil.Interop.Tasks;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Options;
using BUtil.Core.Services;
using BUtil.UI;
using BUtil.UI.Tasks.Controls;
using System;
using System.Collections.Generic;

namespace BUtil.Tasks.BUtilClient.UI.Controls;

public class EditBUtilServerClientTaskViewModel : BUtil.UI.Controls.ViewModelBase
{
    private readonly string _taskName;

    public EditBUtilServerClientTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;
        IsNew = isNew;

        var storeService = new TaskStore(new LocalFileSystem());
        var task = isNew
            ? new TaskV2 { Name = taskName, Model = new BUtilClientModelOptionsV2(string.Empty, FileSenderDirection.ToServer, new FolderStorageSettingsV2(), false) }
            : storeService.Load(taskName) ?? new TaskV2 { Name = taskName, Model = new BUtilClientModelOptionsV2(string.Empty, FileSenderDirection.ToServer, new FolderStorageSettingsV2(), false) };
        TaskIdentityViewModel = new TaskIdentityViewModel(isNew, task.Model, task.Name);
        TaskIdentityViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(TaskIdentityViewModel.Name))
                FormErrorsText = null;
        };
        SetWindowTitleForEdit(taskName, isNew);
        var model = (BUtilClientModelOptionsV2)task.Model;

        var schedule = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
        WhenTaskViewModel = new BUtil.UI.Controls.WhenTaskViewModel(isNew ? new ScheduleInfo() : schedule.GetSchedule(taskName) ?? new ScheduleInfo(), isNew);

        FolderSectionViewModel = new FolderSectionViewModel(model.Folder, model.SkipExistingFiles, isNew);
        FolderSectionViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(FolderSectionViewModel.Folder)
                or nameof(FolderSectionViewModel.Error))
                FormErrorsText = null;
        };
        StorageViewModel = new BUtil.UI.Controls.StorageViewModel(model.To, Resources.LeftMenu_Where, isNew, Resources.UploadFolderTask_Storage_Help);
        StorageViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(BUtil.UI.Controls.StorageViewModel.SelectedProvider)
                or nameof(BUtil.UI.Controls.StorageViewModel.Quota)
                or nameof(BUtil.UI.Controls.StorageViewModel.Error))
                FormErrorsText = null;
        };
    }

    public bool IsNew { get; set; }
    public TaskIdentityViewModel TaskIdentityViewModel { get; }
    public BUtil.UI.Controls.WhenTaskViewModel WhenTaskViewModel { get; }
    public FolderSectionViewModel FolderSectionViewModel { get; }
    public BUtil.UI.Controls.StorageViewModel StorageViewModel { get; }

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

    #region Commands

#pragma warning disable CA1822
    public void ButtonCancelCommand()
#pragma warning restore CA1822
    {
        TaskUINavigation.ReturnToTasksList();
    }

    public void ButtonOkCommand()
    {
        FormErrorsText = null;
        var errors = new List<string>();
        if (!TaskIdentityViewModel.Validate(IsNew ? null : _taskName))
            errors.Add($"{Resources.Name_Title}: {TaskIdentityViewModel.NameError}");
        if (!FolderSectionViewModel.Validate())
            errors.Add($"{Resources.LeftMenu_What}: {FolderSectionViewModel.Error}");
        if (!StorageViewModel.Validate())
            errors.Add($"{StorageViewModel.Title}: {StorageViewModel.Error}");
        if (errors.Count > 0)
        {
            FormErrorsText = string.Join(Environment.NewLine, errors);
            return;
        }

        var newTask = new TaskV2
        {
            Name = TaskIdentityViewModel.Name.TrimEnd(),
            Model = new BUtilClientModelOptionsV2(FolderSectionViewModel.Folder, FileSenderDirection.ToServer, StorageViewModel.GetStorageSettings(), FolderSectionViewModel.SkipExistingFiles)
        };

        if (!TaskV2Validator.TryValidate(newTask, true, IsNew ? null : _taskName, out var error))
        {
            var detectedInfo = StorageViewModel.ApplyDetectedConnectionTrustAndBuildInfo(((BUtilClientModelOptionsV2)newTask.Model).To);
            if (!string.IsNullOrWhiteSpace(detectedInfo))
            {
                FormErrorsText = detectedInfo;
                return;
            }
            StorageViewModel.ApplyExternalError(error);
            FormErrorsText = $"{StorageViewModel.Title}: {error}";
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
