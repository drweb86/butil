using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Synchronization;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.Synchronization;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.IncrementalModel;

class SynchronizationRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly SynchronizationServices _synchronizationServices;

    public SynchronizationRootTask(ILog log, TaskEvents backupEvents, TaskV2 task)
        : base(log, backupEvents, Resources.IncrementalBackup_Title, null)
    {
        _commonServicesIoc = new CommonServicesIoc();

        var options = (SynchronizationTaskModelOptionsV2)task.Model;
        _synchronizationServices = new SynchronizationServices(log, task.Name, options.LocalFolder, options.To, false);

        var tasks = new List<BuTask>();
        tasks.Add(new SynchronizationAllStatesReadTask(_synchronizationServices, Events, task));

        //tasks.Add(new WriteIncrementedVersionTask(_storageService, Events, readSatesTask.StorageStateTask, readSatesTask.GetSourceItemStateTasks, (IncrementalBackupModelOptionsV2)backupTask.Model));

        // 2. приведение в консистестентное состояние файлов удаленных в сооответствиие с состоянием.
        // ищем файлы с определенным расширением. ?????????
        // 3. решаем по поводу как должны проходить синхронизация
        // вариант 1 - удаленная
        // вариант 2 - локальная + обычная
        // вариант 3 - обычная
        // 4. собственно делаем синхронизацию


        Children = tasks;
    }

    public override void Execute()
    {
        Events.OnMessage += OnAddLastMinuteLogMessage;
        UpdateStatus(ProcessingStatus.InProgress);
        base.Execute();

        _commonServicesIoc.Dispose();

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        Events.OnMessage -= OnAddLastMinuteLogMessage;
        PutLastMinuteLogMessages();

        _synchronizationServices.Dispose();
    }

    private void PutLastMinuteLogMessages()
    {
        foreach (var lastMinuteLogMessage in _lastMinuteLogMessages)
            Log.WriteLine(LoggingEvent.Debug, lastMinuteLogMessage);
    }

    private readonly List<string> _lastMinuteLogMessages = new();
    private void OnAddLastMinuteLogMessage(object? sender, MessageEventArgs e)
    {
        _lastMinuteLogMessages.Add(e.Message);
    }
}
