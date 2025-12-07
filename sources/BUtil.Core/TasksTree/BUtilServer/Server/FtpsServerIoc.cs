using BUtil.Core.Logs;
using BUtil.Core.Services;
using FtpsServerLibrary;
using System;

namespace BUtil.Core.TasksTree.BUtilServer.Server;
public class FtpsServerIoc
{
    public CommonServicesIoc Common {  get; set; }

    public FtpsServerIoc(ILog log, Action<string?> onGetLastMinuteMessage)
    {
        Common = new CommonServicesIoc(log, onGetLastMinuteMessage);
    }

    public FtpsServer Server { get; set; } = null!;
    public void Dispose()
    {
        Server?.Stop();
        Common.Dispose();
    }
}
