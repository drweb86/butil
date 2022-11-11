using BUtil.Core.Logs;
using BUtil.Core.Options;
using BUtil.Core.Storages;
using System;
using System.Text.Json;

namespace BUtil.Core.BackupModels
{
    public static class BackupModelStrategyFactory
    {
        public static IBackupModelStrategy Create(ILog log, BackupTask task, ProgramOptions options)
        {
            switch (task.BackupModel.ProviderName)
            {
                case BackupModelProviderNames.Incremental:
                    var modelOptions = CreateIncrementalBackupModelOptions(task.BackupModel);
                    return new IncrementalBackupModelStrategy(log, task, modelOptions, options);
                default:
                    throw new ArgumentOutOfRangeException(nameof(task));
            }
        }

        public static IncrementalBackupModelOptions CreateIncrementalBackupModelOptions(BackupModel backupModel)
        {
            return JsonSerializer.Deserialize<IncrementalBackupModelOptions>(backupModel.Options);
        }

        public static BackupModel CreateBackupModel(IncrementalBackupModelOptions settings)
        {
            return new BackupModel
            {
                ProviderName = BackupModelProviderNames.Incremental,
                Options = JsonSerializer.Serialize(settings)
            };
        }
    }
}
