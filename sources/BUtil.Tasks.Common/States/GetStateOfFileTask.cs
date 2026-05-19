using BUtil.Interop.Tasks;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Interop.Tasks.Events;
using BUtil.Interop.Logs;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Interop.Tasks.Core;
using System.IO;

namespace BUtil.Tasks.Common.States;

internal class GetStateOfFileTask : BuTaskV2
{
    private readonly CommonServicesIoc _servicesIoc;
    private readonly string _fileName;
    public FileState? State { get; private set; }

    public GetStateOfFileTask(TaskEvents events, CommonServicesIoc servicesIoc, SourceItemV2 item, string fileName) :
        base(servicesIoc.Log, events, string.Empty)
    {
        _servicesIoc = servicesIoc;
        _fileName = fileName;
        Title = string.Format(BUtil.Core.Localization.Resources.State_File_Get, SourceItemHelper.GetFriendlyFileName(item, fileName));
    }

    protected override void ExecuteInternal()
    {
        var fileInfo = new FileInfo(_fileName);
        State = new FileState(_fileName, fileInfo.LastWriteTimeUtc, fileInfo.Length, _servicesIoc.CachedHashService.GetSha512(_fileName, true));
    }
}
