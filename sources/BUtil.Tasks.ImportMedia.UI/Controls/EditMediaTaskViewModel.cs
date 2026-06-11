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
using System.Threading.Tasks;

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
        SetWindowTitleForEdit(taskName, isNew);
        var model = (ImportMediaTaskModelOptionsV2)task.Model;

        ImportMediaTaskWhereTaskViewModel = new BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel(model.DestinationFolder, model.SkipAlreadyImportedFiles, model.DeleteCopiedDataOnSourceMedia, model.TransformFileName, model.FileLastWriteTimeMin, isNew);
        SourceTaskViewModel = new BUtil.UI.Controls.StorageViewModel(model.From, Resources.LeftMenu_What, "/Assets/CrystalProject_EveraldoCoelho_SourceItems48x48.png", isNew);
    }

    public BUtil.UI.Controls.ImportMediaTaskWhereTaskViewModel ImportMediaTaskWhereTaskViewModel { get; }
    public TaskIdentityViewModel TaskIdentityViewModel { get; }
    public BUtil.UI.Controls.StorageViewModel SourceTaskViewModel { get; }
    public bool IsNew { get; set; }

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

    public async Task ButtonOkCommand()
    {
        var newTask = new TaskV2
        {
            Name = TaskIdentityViewModel.Name.TrimEnd(),
            Model = new ImportMediaTaskModelOptionsV2
            {
                DestinationFolder = ImportMediaTaskWhereTaskViewModel.OutputFolder,
                SkipAlreadyImportedFiles = ImportMediaTaskWhereTaskViewModel.SkipAlreadyImportedFiles,
                DeleteCopiedDataOnSourceMedia = ImportMediaTaskWhereTaskViewModel.DeleteCopiedDataOnSourceMedia,
                FileLastWriteTimeMin = ImportMediaTaskWhereTaskViewModel.FileLastWriteTimeMin?.DateTime ?? null,
                TransformFileName = ImportMediaTaskWhereTaskViewModel.TransformFileName,
                From = SourceTaskViewModel.GetStorageSettings()
            }
        };

        if (!TaskV2Validator.TryValidate(newTask, true, IsNew ? null : _taskName, out var error))
        {
            var detectedInfo = SourceTaskViewModel.ApplyDetectedConnectionTrustAndBuildInfo(((ImportMediaTaskModelOptionsV2)newTask.Model).From);
            if (!string.IsNullOrWhiteSpace(detectedInfo))
            {
                await Messages.ShowInformationBox(detectedInfo);
                return;
            }
            await Messages.ShowErrorBox(error);
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
            await Messages.ShowErrorBox(ExceptionHelper.ToString(e));
            return;
        }

        TaskUINavigation.ReturnToTasksList();
    }

    #endregion

    private readonly string _taskName;
}
