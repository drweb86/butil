using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks;
using BUtil.Tasks.ImportMedia;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.UI;
using BUtil.UI.Tasks.Controls;
using System;
using System.Collections.Generic;

namespace BUtil.Tasks.ImportMedia.UI.Controls;

public class EditMediaTaskViewModel : BUtil.UI.Controls.ViewModelBase
{
    public EditMediaTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;
        IsNew = isNew;

        var storeService = new TaskStore(new LocalFileSystem());
        var task = isNew
            ? new TaskV2 { Name = taskName, Model = new ImportMediaTaskModelOptionsV2() }
            : storeService.Load(taskName) ?? new TaskV2() { Model = new ImportMediaTaskModelOptionsV2() };
        TaskIdentityViewModel = new TaskIdentityViewModel(isNew, task.Model, task.Name);
        TaskIdentityViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(TaskIdentityViewModel.Name))
                FormErrorsText = null;
        };
        SetWindowTitleForEdit(taskName, isNew);
        var model = (ImportMediaTaskModelOptionsV2)task.Model;

        ImportMediaTaskWhereTaskViewModel = new BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel(
            model.DestinationFolder,
            model.SkipAlreadyImportedFiles,
            model.DeleteCopiedDataOnSourceMedia,
            model.TransformFileName,
            model.FileLastWriteTimeMin,
            model.FileExtensions,
            isNew,
            isNew);
        ImportMediaTaskWhereTaskViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel.OutputFolder)
                or nameof(BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel.TransformFileName)
                or nameof(BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel.FileExtensionsText)
                or nameof(BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel.Error))
                FormErrorsText = null;
        };
        SourceTaskViewModel = new BUtil.UI.Controls.StorageViewModel(model.From, Resources.LeftMenu_What, isNew, Resources.ImportMediaTask_Storage_Help);
        SourceTaskViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(BUtil.UI.Controls.StorageViewModel.SelectedProvider)
                or nameof(BUtil.UI.Controls.StorageViewModel.Quota)
                or nameof(BUtil.UI.Controls.StorageViewModel.Error))
                FormErrorsText = null;
        };
    }

    public BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel ImportMediaTaskWhereTaskViewModel { get; }
    public TaskIdentityViewModel TaskIdentityViewModel { get; }
    public BUtil.UI.Controls.StorageViewModel SourceTaskViewModel { get; }
    public bool IsNew { get; set; }

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

    #region Labels
    public static string Button_Cancel => Resources.Button_Cancel;
    public static string Button_OK => Resources.Button_OK;
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
        if (!SourceTaskViewModel.Validate())
            errors.Add($"{SourceTaskViewModel.Title}: {SourceTaskViewModel.Error}");
        if (!ImportMediaTaskWhereTaskViewModel.Validate())
            errors.Add($"{Resources.LeftMenu_Where}: {ImportMediaTaskWhereTaskViewModel.Error}");
        if (errors.Count > 0)
        {
            FormErrorsText = string.Join(Environment.NewLine, errors);
            return;
        }

        var fileExtensions = ImportMediaTaskWhereTaskViewModel.GetFileExtensions();
        var newTask = new TaskV2
        {
            Name = TaskIdentityViewModel.Name.TrimEnd(),
            Model = new ImportMediaTaskModelOptionsV2
            {
                DestinationFolder = ImportMediaTaskWhereTaskViewModel.OutputFolder,
                SkipAlreadyImportedFiles = ImportMediaTaskWhereTaskViewModel.SkipAlreadyImportedFiles,
                DeleteCopiedDataOnSourceMedia = ImportMediaTaskWhereTaskViewModel.DeleteCopiedDataOnSourceMedia,
                FileLastWriteTimeMin = ImportMediaTaskWhereTaskViewModel.FileLastWriteTimeMin,
                TransformFileName = ImportMediaTaskWhereTaskViewModel.TransformFileName,
                FileExtensions = fileExtensions.Count == 0 ? null : fileExtensions,
                From = SourceTaskViewModel.GetStorageSettings()
            }
        };

        if (!TaskV2Validator.TryValidate(newTask, true, IsNew ? null : _taskName, out var error))
        {
            var detectedInfo = SourceTaskViewModel.ApplyDetectedConnectionTrustAndBuildInfo(((ImportMediaTaskModelOptionsV2)newTask.Model).From);
            if (!string.IsNullOrWhiteSpace(detectedInfo))
            {
                FormErrorsText = detectedInfo;
                return;
            }
            SourceTaskViewModel.ApplyExternalError(error);
            FormErrorsText = $"{SourceTaskViewModel.Title}: {error}";
            return;
        }

        var storeService = new TaskStore(new LocalFileSystem());
        if (!IsNew)
        {
            storeService.Delete(_taskName);
            LogService.MoveLogs(_taskName, newTask.Name);
            ImportMediaFileService.MoveState(_taskName, newTask.Name);
        }
        try
        {
            storeService.Save(newTask);
        }
        catch (Exception e)
        {
            FormErrorsText = ExceptionHelper.ToString(e);
            return;
        }

        TaskUINavigation.ReturnToTasksList();
    }

    #endregion

    private readonly string _taskName;
}
