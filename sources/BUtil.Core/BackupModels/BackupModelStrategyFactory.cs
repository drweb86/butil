using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using System;
using System.IO;

namespace BUtil.Core.BackupModels
{
    public static class BackupModelStrategyFactory
    {
        public static IBackupModelStrategy Create(ILog log, BackupTaskV2 task)
        {
            if (task.Model is IncrementalBackupModelOptionsV2)
                return new IncrementalBackupModelStrategy(log, task);
            if (task.Model is ImportMediaBackupModelOptionsV2)
                return new ImportMediaBackupModelStrategy(log, task);
            throw new ArgumentOutOfRangeException(nameof(task));
        }

        public static bool TryVerify(ILog log, IBackupModelOptionsV2 options, out string error)
        {
            error = null;
            if (options is IncrementalBackupModelOptionsV2)
            {
                var typedOptions = (IncrementalBackupModelOptionsV2)options;
                if (typedOptions.Items.Count == 0)
                {
                    error = Resources.ThereAreNoItemsToBackup;
                    return false;
                }

                foreach (var item in typedOptions.Items)
                {
                    if (item.IsFolder)
                    {
                        if (!Directory.Exists(item.Target))
                        {
                            error = string.Format(Resources.SourceItemDoesNotExist, item.Target);
                            return false;
                        }
                    }
                    else
                    {
                        if (!File.Exists(item.Target))
                        {
                            error = string.Format(Resources.SourceItemDoesNotExist, item.Target);
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
            } else if (options is ImportMediaBackupModelOptionsV2)
            {
                var typedOptions = (ImportMediaBackupModelOptionsV2)options;

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
                    error = BUtil.Core.Localization.Resources.TransformFileNameIsEmpty;
                    return false;
                }

                try
                {
                    DateTokenReplacer.ParseString(typedOptions.TransformFileName, DateTime.Now);
                }
                catch
                {
                    error = BUtil.Core.Localization.Resources.InvalidTransformFileNameString;
                    return false;
                }


                return true;
            }

            throw new ArgumentOutOfRangeException(nameof(options));
        }
    }
}
