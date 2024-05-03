using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.TasksTree.Core;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.IncrementalModel;

class SynchronizationRootTask : SequentialBuTask
{
    private readonly CommonServicesIoc _commonServicesIoc;
    private readonly StorageSpecificServicesIoc _storageService;

    public SynchronizationRootTask(ILog log, TaskEvents backupEvents, TaskV2 task)
        : base(log, backupEvents, Resources.IncrementalBackup_Title, null)
    {

        _commonServicesIoc = new CommonServicesIoc();
        var modelOptions = (IncrementalBackupModelOptionsV2)task.Model;
        var storage = modelOptions.To;

        _storageService = new StorageSpecificServicesIoc(Log, storage, _commonServicesIoc.HashService);

        var tasks = new List<BuTask>();
        //tasks.Add(readSatesTask);

        //tasks.Add(new WriteIncrementedVersionTask(_storageService, Events, readSatesTask.StorageStateTask, readSatesTask.GetSourceItemStateTasks, (IncrementalBackupModelOptionsV2)backupTask.Model));

        // 1. прочитать состояние актуальное, сохраненное, удаленное // 3 ! параллельно
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

        _storageService.Dispose();
        _commonServicesIoc.Dispose();

        UpdateStatus(IsSuccess ? ProcessingStatus.FinishedSuccesfully : ProcessingStatus.FinishedWithErrors);
        Events.OnMessage -= OnAddLastMinuteLogMessage;
        PutLastMinuteLogMessages();
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
