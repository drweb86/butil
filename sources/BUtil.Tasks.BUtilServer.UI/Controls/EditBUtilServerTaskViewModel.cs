using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks;
using BUtil.Core;
using BUtil.Tasks.BUtilServer;
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

namespace BUtil.Tasks.BUtilServer.UI.Controls;

public class EditBUtilServerTaskViewModel : BUtil.UI.Controls.ViewModelBase
{
    private readonly string _taskName;

    public EditBUtilServerTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;
        IsNew = isNew;

        var storeService = new TaskStore(new LocalFileSystem());
        var task = isNew
            ? new TaskV2 { Name = taskName, Model = new BUtilServerModelOptionsV2 { Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) } }
            : storeService.Load(taskName) ?? new TaskV2 { Name = taskName, Model = new BUtilServerModelOptionsV2 { Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) } };
        TaskIdentityViewModel = new TaskIdentityViewModel(isNew, task.Model, task.Name);
        TaskIdentityViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(TaskIdentityViewModel.Name))
                FormErrorsText = null;
        };
        SetWindowTitleForEdit(taskName, isNew);
        var model = (BUtilServerModelOptionsV2)task.Model;
        var schedule = PlatformSpecificExperience.Instance.GetTaskSchedulerService();
        WhenTaskViewModel = new BUtil.UI.Controls.WhenTaskViewModel(isNew ? new ScheduleInfo() : schedule.GetSchedule(taskName) ?? new ScheduleInfo(), isNew);
        FtpsServerConfigurationViewModel = new BUtil.UI.Controls.FtpsServerConfigurationViewModel(model.Port, model.Username, model.Password, model.Folder, model.DurationMinutes, model.FolderAccess, isNew);
        FtpsServerConfigurationViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(BUtil.UI.Controls.FtpsServerConfigurationViewModel.Port)
                or nameof(BUtil.UI.Controls.FtpsServerConfigurationViewModel.FtpsUser)
                or nameof(BUtil.UI.Controls.FtpsServerConfigurationViewModel.FtpsPassword)
                or nameof(BUtil.UI.Controls.FtpsServerConfigurationViewModel.Folder)
                or nameof(BUtil.UI.Controls.FtpsServerConfigurationViewModel.FolderAccess)
                or nameof(BUtil.UI.Controls.FtpsServerConfigurationViewModel.DurationMinutes)
                or nameof(BUtil.UI.Controls.FtpsServerConfigurationViewModel.Error))
                FormErrorsText = null;
        };
    }

    public bool IsNew { get; set; }
    public TaskIdentityViewModel TaskIdentityViewModel { get; }
    public BUtil.UI.Controls.WhenTaskViewModel WhenTaskViewModel { get; }
    public BUtil.UI.Controls.FtpsServerConfigurationViewModel FtpsServerConfigurationViewModel { get; }

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
        if (!FtpsServerConfigurationViewModel.Validate())
            errors.Add($"{Resources.FtpsServerConfiguration_Title}: {FtpsServerConfigurationViewModel.Error}");
        if (errors.Count > 0)
        {
            FormErrorsText = string.Join(Environment.NewLine, errors);
            return;
        }

        var newTask = new TaskV2
        {
            Name = TaskIdentityViewModel.Name.TrimEnd(),
            Model = new BUtilServerModelOptionsV2(
                (int)FtpsServerConfigurationViewModel.Port,
                FtpsServerConfigurationViewModel.FtpsUser!,
                FtpsServerConfigurationViewModel.FtpsPassword!,
                FtpsServerConfigurationViewModel.Folder,
                FtpsServerConfigurationViewModel.DurationMinutes,
                FtpsServerConfigurationViewModel.FolderAccess)
        };

        if (!TaskV2Validator.TryValidate(newTask, true, IsNew ? null : _taskName, out var error))
        {
            FormErrorsText = error;
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
