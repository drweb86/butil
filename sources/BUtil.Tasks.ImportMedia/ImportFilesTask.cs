using BUtil.Interop.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Tasks.ImportMedia;
using BUtil.Interop.Tasks.Events;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Interop.Tasks.Core;
using BUtil.Tasks.Common.Storage;
using BUtil.Tasks.Common.States;

namespace BUtil.Core.TasksTree.ImportMedia;

class ImportFilesTask : SequentialBuTask
{
    private readonly TaskV2 _task;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly ImportMediaTaskModelOptionsV2 _options;

    public ImportFilesTask(TaskEvents backupEvents, TaskV2 backupTask, GetStateOfSourceItemTask getStateOfSourceItemTask, CommonServicesIoc commonServicesIoc)
        : base(commonServicesIoc.Log, backupEvents, Localization.Resources.ImportMediaTask_AllFiles)
    {
        Children = [];
        _task = backupTask;
        _getStateOfSourceItemTask = getStateOfSourceItemTask;
        _commonServicesIoc = commonServicesIoc;
        _options = (ImportMediaTaskModelOptionsV2)_task.Model;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        using var fromStorageServices = new StorageSpecificServicesIoc(_commonServicesIoc, _options.From);
        using var toStorage = StorageFactory.Create(this.Log, new FolderStorageSettingsV2 { DestinationFolder = _options.DestinationFolder }, false);
        var fromStorage = fromStorageServices.Storage;

        var fromStorageFiles = fromStorage.GetFiles(null, SearchOption.AllDirectories);
        var state = GetState();

        var allTasks = new List<BuTask>();

        var importTasks = ImportFiles(toStorage, fromStorage, fromStorageFiles, state, allTasks);
        DeleteCopiedDataOnSourceMedia(fromStorageServices, importTasks, allTasks);

        state.Files.AddRange(importTasks.Where(x => x.IsSuccess).Select(x => x.File));
        SaveState(state);
        UpdateStatus(allTasks.All(x => x.IsSuccess) ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }

    private List<ImportSingleFileTask> ImportFiles(IStorage toStorage, IStorage fromStorage, string[] fromStorageFiles, ImportMediaState state, List<BuTask> allTasks)
    {
        var fromStorageFilesToProcess = fromStorageFiles.Where(x => !state.Files.Contains(x)).ToList();

        var importTasks = fromStorageFilesToProcess
            .Select(x => new ImportSingleFileTask(Events, _options.FileLastWriteTimeMin, x, fromStorage, toStorage, _options.TransformFileName, _getStateOfSourceItemTask.SourceItemState ?? throw new InvalidOperationException(), _commonServicesIoc))
            .ToList();
        allTasks.AddRange(importTasks);
        Events.DuringExecutionTasksAdded(Id, importTasks);
        Children = importTasks;
        base.Execute();
        return importTasks;
    }

    private void SaveState(ImportMediaState state)
    {
        if (!_options.SkipAlreadyImportedFiles)
            return;

        new ImportMediaFileService()
            .Save(state, _task.Name);
    }

    private ImportMediaState GetState()
    {
        return _options.SkipAlreadyImportedFiles 
            ? new ImportMediaFileService().Load(_task.Name) ?? new ImportMediaState()
            : new ImportMediaState();
    }

    private void DeleteCopiedDataOnSourceMedia(StorageSpecificServicesIoc fromStorageServices,
        List<ImportSingleFileTask> importTasks,
        List<BuTask> allTasks)
    {
        if (!_options.DeleteCopiedDataOnSourceMedia)
            return;

        var importedTasks = importTasks
            .Where(x => x.IsSuccess && !x.IsSkipped)
            .ToList();

        if (!importedTasks.Any())
            return;

        var deleteTasks = importedTasks
            .Select(x => new DeleteStorageFileTask(fromStorageServices, Events, x.File))
            .ToList();

        Events.DuringExecutionTasksAdded(importedTasks.Last().Id, deleteTasks);
        Children = deleteTasks;
        allTasks.AddRange(deleteTasks);
        base.Execute();
    }
}
