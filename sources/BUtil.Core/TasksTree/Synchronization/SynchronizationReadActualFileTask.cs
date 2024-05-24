using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using System;

namespace BUtil.Core.TasksTree.Synchronization;
internal class SynchronizationReadActualFileTask : BuTaskV2
{
    private readonly SynchronizationServices _synchronizationServices;
    private readonly string _file;

    public SynchronizationStateFile? StateFile { get; private set; }

    public SynchronizationReadActualFileTask(SynchronizationServices synchronizationServices, TaskEvents events, string localFolder, string fullPath)
        : base(synchronizationServices.Log, events, string.Format(Localization.Resources.State_File_Get, SourceItemHelper.GetFriendlyFileName(new ConfigurationFileModels.V2.SourceItemV2(localFolder, true), fullPath)))
    {
        _synchronizationServices = synchronizationServices;
        _file = fullPath;
    }

    protected override void ExecuteInternal()
    {
        StateFile = _synchronizationServices.ActualFilesService.CalculateItem(_file);
    }
}
