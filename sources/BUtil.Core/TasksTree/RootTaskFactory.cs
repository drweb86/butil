using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FileSystem;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Storages;
using BUtil.Core.TasksTree.BUtilServer.Client;
using BUtil.Core.TasksTree.BUtilServer.Server;
using BUtil.Core.TasksTree.Core;
using BUtil.Core.TasksTree.IncrementalModel;
using BUtil.Core.TasksTree.MediaSyncBackupModel;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace BUtil.Core.TasksTree;

public static class RootTaskFactory
{
    public static BuTask Create(ILog log, TaskV2 task, Events.TaskEvents events, Action<string?> onGetLastMinuteMessage)
    {
        if (task.Model is IncrementalBackupModelOptionsV2)
            return new IncrementalBackupRootTask(log, events, task, onGetLastMinuteMessage);
        if (task.Model is SynchronizationTaskModelOptionsV2)
            return new SynchronizationRootTask(log, events, task, onGetLastMinuteMessage);
        if (task.Model is ImportMediaTaskModelOptionsV2)
            return new ImportMediaRootTask(log, events, task, onGetLastMinuteMessage);
        if (task.Model is BUtilServerModelOptionsV2)
            return new FtpsServerRootTask(log, events, task, onGetLastMinuteMessage);
        if (task.Model is BUtilClientModelOptionsV2)
            return new BUtilClientRootTask(log, events, task, onGetLastMinuteMessage);
        throw new ArgumentOutOfRangeException(nameof(task));
    }

    public static bool TryVerify(ILog log, ITaskModelOptionsV2 options, bool writeMode, [NotNullWhen(false)] out string? error)
    {
        error = null;
        if (options is IncrementalBackupModelOptionsV2)
        {
            var typedOptions = (IncrementalBackupModelOptionsV2)options;

            if (string.IsNullOrWhiteSpace(typedOptions.Password))
            {
                error = Resources.Password_Field_Validation_NotSpecified;
                return false;
            }

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
                    // temporary
                    error = $"Please edit task and remove source item {item.Target}. Its support is dropped. You can add folders only";
                    return false;
                }
            }

            var storageError = StorageFactory.Test(log, typedOptions.To, writeMode);
            if (storageError != null)
            {
                error = storageError;
                return false;
            }

            return true;
        }
        else if (options is SynchronizationTaskModelOptionsV2)
        {
            var typedOptions = (SynchronizationTaskModelOptionsV2)options;

            if (string.IsNullOrWhiteSpace(typedOptions.Password))
            {
                error = Resources.Password_Field_Validation_NotSpecified;
                return false;
            }

            if (string.IsNullOrWhiteSpace(typedOptions.LocalFolder) ||
                !Directory.Exists(typedOptions.LocalFolder))
            {
                error = string.Format(Resources.SourceItem_Validation_NotExists, typedOptions.LocalFolder);
                return false;
            }

            var storageError = StorageFactory.Test(log, typedOptions.To, writeMode && typedOptions.SynchronizationMode == SynchronizationTaskModelMode.TwoWay);
            if (storageError != null)
            {
                error = storageError;
                return false;
            }

            return true;
        }
        else if (options is ImportMediaTaskModelOptionsV2 typedOptions)
        {
            var storageError = StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = typedOptions.DestinationFolder }, writeMode);
            if (storageError != null)
            {
                error = storageError;
                return false;
            }

            var storageError2 = StorageFactory.Test(log, typedOptions.From, false);
            if (storageError2 != null)
            {
                error = storageError2;
                return false;
            }

            if (string.IsNullOrWhiteSpace(typedOptions.TransformFileName))
            {
                error = Resources.ImportMediaTask_Field_TransformFileName_Validation_Empty;
                return false;
            }

            try
            {
                var str = DateTokenReplacer.ParseString(typedOptions.TransformFileName, DateTime.Now);
                using var tempFolder = new TempFolder();
                var fullPath = Path.Combine(tempFolder.Folder, str);
                Directory.CreateDirectory(fullPath);
            }
            catch
            {
                error = Resources.ImportMediaTask_Field_TransformFileName_Validation_Invalid;
                return false;
            }

            return true;
        }
        else if (options is BUtilServerModelOptionsV2 fileSenderServerOptions)
        {
            if (string.IsNullOrWhiteSpace(fileSenderServerOptions.ServerAddress))
            {
                error = Resources.Server_Field_Address_Validation;
                return false;
            }

            if (string.IsNullOrWhiteSpace(fileSenderServerOptions.Username))
            {
                error = Resources.User_Field_Validation;
                return false;
            }

            if (string.IsNullOrWhiteSpace(fileSenderServerOptions.Password))
            {
                error = Resources.Password_Field_Validation_NotSpecified;
                return false;
            }


            var storageError = StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = fileSenderServerOptions.Folder }, writeMode);
            if (storageError != null)
            {
                error = storageError;
                return false;
            }

            if (fileSenderServerOptions.Port < PlatformSpecificExperience.Instance.MinimumListenerPort)
            {
                error = Resources.Server_Field_Port_Validation + $"(Min port {PlatformSpecificExperience.Instance.MinimumListenerPort})";
                return false;
            }

            return true;
        }
        else if (options is BUtilClientModelOptionsV2 fileSenderTransferOptions)
        {
            var storageError = StorageFactory.Test(log, fileSenderTransferOptions.To, writeMode);
            if (storageError != null)
            {
                error = storageError;
                return false;
            }


            var storageError2 = StorageFactory.Test(log, new FolderStorageSettingsV2 { DestinationFolder = fileSenderTransferOptions.Folder }, writeMode);
            if (storageError2 != null)
            {
                error = storageError2;
                return false;
            }
            
            return true;
        }

        throw new ArgumentOutOfRangeException(nameof(options));
    }
}
