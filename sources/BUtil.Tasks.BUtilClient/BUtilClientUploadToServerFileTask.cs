using BUtil.Tasks.BUtilClient;
using BUtil.Interop.Tasks.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Interop.Tasks.Core;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientUploadToServerFileTask(BUtilClientIoc ioc, TaskEvents taskEvents, BUtilClientModelOptionsV2 options, FileState fileState) : BuTaskV2(ioc.Common.Log, taskEvents, string.Format(Resources.File_Uploading, SourceItemHelper.GetSourceItemRelativeFileName(options.Folder, fileState)))
{
    protected override void ExecuteInternal()
    {
        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(options.Folder, fileState);
        LogDebug($"{relativeFileName}");

        if (options.SkipExistingFiles && ioc.StorageSpecificServices.Storage.Exists(relativeFileName))
        {
            LogDebug($"Skipped (already exists): {relativeFileName}");
            IsSkipped = true;
            return;
        }

        ioc.StorageSpecificServices.Storage.Upload(fileState.FileName, relativeFileName);
    }
}
