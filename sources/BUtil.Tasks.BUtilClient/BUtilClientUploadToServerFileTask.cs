using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Events;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.Core;

namespace BUtil.Core.TasksTree.BUtilServer.Client;

internal class BUtilClientUploadToServerFileTask(BUtilClientIoc ioc, TaskEvents taskEvents, BUtilClientModelOptionsV2 options, FileState fileState) : BuTaskV2(ioc.Common.Log, taskEvents, string.Format(Resources.File_Uploading, SourceItemHelper.GetSourceItemRelativeFileName(options.Folder, fileState)))
{
    protected override void ExecuteInternal()
    {
        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(options.Folder, fileState);
        LogDebug($"{relativeFileName}");

        ioc.StorageSpecificServices.Storage.Upload(fileState.FileName, relativeFileName);
    }
}
