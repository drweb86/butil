using BUtil.Core.Logs;
using BUtil.Core.Services;
using FtpsServerLibrary;
using System;

namespace BUtil.Core.TasksTree.BUtilServer.Server;
public class FtpsServerIoc(ILog log, Action<string?> onGetLastMinuteMessage)
{
    public CommonServicesIoc Common { get; set; } = new CommonServicesIoc(log, onGetLastMinuteMessage);

    public FtpsServer Server { get; set; } = null!;
    public void Dispose()
    {
        Server?.Stop();
        Common.Dispose();
    }
}
