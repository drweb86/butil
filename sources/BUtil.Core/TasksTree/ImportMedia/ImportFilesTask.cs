
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BUtil.Core.TasksTree.MediaSyncBackupModel;

class ImportFilesTask : SequentialBuTask
{
    private readonly TaskV2 _task;
    private readonly GetStateOfSourceItemTask _getStateOfSourceItemTask;
    private readonly CommonServicesIoc _commonServicesIoc;

    public ImportFilesTask(TaskEvents backupEvents, TaskV2 backupTask, GetStateOfSourceItemTask getStateOfSourceItemTask, CommonServicesIoc commonServicesIoc)
        : base(commonServicesIoc.Log, backupEvents, BUtil.Core.Localization.Resources.ImportMediaTask_AllFiles)
    {
        Children = [];
        _task = backupTask;
        _getStateOfSourceItemTask = getStateOfSourceItemTask;
        _commonServicesIoc = commonServicesIoc;
    }

    public override void Execute()
    {
        UpdateStatus(ProcessingStatus.InProgress);

        var options = (ImportMediaTaskModelOptionsV2)_task.Model;

        var importMediaFileService = new ImportMediaFileService();
        var importMediaState = options.SkipAlreadyImportedFiles ? importMediaFileService.Load(_task.Name) ?? new ImportMediaState() : new ImportMediaState();
        var fromStorage = StorageFactory.Create(this.Log, options.From, false);
        var toStorage = StorageFactory.Create(this.Log, new FolderStorageSettingsV2 { DestinationFolder = options.DestinationFolder }, false);
        var transformFileName = options.TransformFileName;

        var fromStorageFiles = fromStorage.GetFiles(null, SearchOption.AllDirectories);
        var fromStorageFilesToProcess = fromStorageFiles.Where(x => !importMediaState.Files.Contains(x)).ToList();

        var tasks = fromStorageFilesToProcess
            .Select(x => new ImportSingleFileTask(Events, x, fromStorage, toStorage, transformFileName, _getStateOfSourceItemTask.SourceItemState ?? throw new InvalidOperationException(), _commonServicesIoc))
            .ToList();
        Events.DuringExecutionTasksAdded(Id, tasks);

        Children = tasks;
        base.Execute();

        importMediaState.Files.AddRange(tasks.Where(x => x.IsSuccess).Select(x => x.File));

        if (options.SkipAlreadyImportedFiles)
        {
            importMediaFileService.Save(importMediaState, _task.Name);
        }
        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
    }
}
