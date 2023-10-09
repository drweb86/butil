
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace BUtil.Core.BackupModels
{
    public static class TaskModelStrategyFactory
    {
        public static ITaskModelStrategy Create(ILog log, TaskV2 task)
        {
            if (task.Model is IncrementalBackupModelOptionsV2)
                return new IncrementalBackupModelStrategy(log, task);
            if (task.Model is ImportMediaTaskModelOptionsV2)
                return new ImportMediaTaskModelStrategy(log, task);
            throw new ArgumentOutOfRangeException(nameof(task));
        }

        public static bool TryVerify(ILog log, ITaskModelOptionsV2 options, [NotNullWhen(false)] out string? error)
        {
            error = null;
            if (options is IncrementalBackupModelOptionsV2)
            {
                var typedOptions = (IncrementalBackupModelOptionsV2)options;
                if (typedOptions.Items.Count == 0)
                {
                    error = Resources.SourceItem_Validation_Empty;
                    return false;
                }

                foreach (var item in typedOptions.Items)
                {
                    if (item.IsFolder)
                    {
                        if (!Directory.Exists(item.Target))
                        {
                            error = string.Format(Resources.SourceItem_Validation_NotExists, item.Target);
                            return false;
                        }
                    }
                    else
                    {
                        if (!File.Exists(item.Target))
                        {
                            error = string.Format(Resources.SourceItem_Validation_NotExists, item.Target);
                            return false;
                        }
                    }
                }

                var storageError = StorageFactory.Test(log, typedOptions.To);
                if (storageError != null)
                {
                    error = storageError;
                    return false;
                }

                return true;
            } else if (options is ImportMediaTaskModelOptionsV2)
            {
                var typedOptions = (ImportMediaTaskModelOptionsV2)options;

                var storageError = StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = typedOptions.DestinationFolder });
                if (storageError != null)
                {
                    error = storageError;
                    return false;
                }

                var storageError2 = StorageFactory.Test(log, typedOptions.From);
                if (storageError2 != null)
                {
                    error = storageError2;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(typedOptions.TransformFileName))
                {
                    error = BUtil.Core.Localization.Resources.ImportMediaTask_Field_TransformFileName_Validation_Empty;
                    return false;
                }

                try
                {
                    DateTokenReplacer.ParseString(typedOptions.TransformFileName, DateTime.Now);
                }
                catch
                {
                    error = BUtil.Core.Localization.Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
                    return false;
                }


                return true;
            }

            throw new ArgumentOutOfRangeException(nameof(options));
        }
    }
}
