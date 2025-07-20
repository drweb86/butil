using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Options;
using BUtil.Core.State;
using butil_ui.ViewModels;
using System;
using System.Threading.Tasks;

namespace butil_ui.Controls;

public class EditMediaTaskViewModel : ViewModelBase
{
    public EditMediaTaskViewModel(string taskName, bool isNew)
    {
        _taskName = taskName;
        WindowTitle = isNew ? Resources.Task_Field_Name_NewDefaultValue : taskName;
        IsNew = isNew;
        NameTaskViewModel = new NameTaskViewModel(isNew, Resources.ImportMediaTask_Help, isNew ? Resources.Task_Field_Name_NewDefaultValue : taskName);

        var storeService = new TaskV2StoreService();
        var task = isNew ? new TaskV2() { Model = new ImportMediaTaskModelOptionsV2() } : storeService.Load(taskName) ?? new TaskV2() { Model = new ImportMediaTaskModelOptionsV2() };
        var model = (ImportMediaTaskModelOptionsV2)task.Model;

        ImportMediaTaskWhereTaskViewModel = new ImportMediaTaskWhereTaskViewModel(model.DestinationFolder, model.SkipAlreadyImportedFiles, model.TransformFileName, model.FileLastWriteTimeMin);
        SourceTaskViewModel = new WhereTaskViewModel(model.From, Resources.LeftMenu_What, "/Assets/CrystalProject_EveraldoCoelho_SourceItems48x48.png");
    }

    public ImportMediaTaskWhereTaskViewModel ImportMediaTaskWhereTaskViewModel { get; }
    public NameTaskViewModel NameTaskViewModel { get; }
    public WhereTaskViewModel SourceTaskViewModel { get; }
    public bool IsNew { get; set; }

    #region Labels
    public static string Button_Cancel => Resources.Button_Cancel;
    public static string Button_OK => Resources.Button_OK;
    #endregion

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
            Name = NameTaskViewModel.Name.TrimEnd(),
            Model = new ImportMediaTaskModelOptionsV2
            {
                DestinationFolder = ImportMediaTaskWhereTaskViewModel.OutputFolder,
                SkipAlreadyImportedFiles = ImportMediaTaskWhereTaskViewModel.SkipAlreadyImportedFiles,
                FileLastWriteTimeMin = ImportMediaTaskWhereTaskViewModel.FileLastWriteTimeMin?.DateTime ?? null,
                TransformFileName = ImportMediaTaskWhereTaskViewModel.TransformFileName,
                From = SourceTaskViewModel.GetStorageSettings()
            }
        };

        if (!TaskV2Validator.TryValidate(newTask, true, out var error))
        {
            await Messages.ShowErrorBox(error);
            return;
        }

        var storeService = new TaskV2StoreService();
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

        WindowManager.SwitchView(new TasksViewModel());
    }

    #endregion

    private readonly string _taskName;
}