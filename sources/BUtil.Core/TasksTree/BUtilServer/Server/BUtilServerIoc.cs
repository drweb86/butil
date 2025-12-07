using BUtil.Core.Logs;
using BUtil.Core.Services;
using FtpsServerLibrary;
using System;

namespace BUtil.Core.TasksTree.BUtilServer.Server;
public class BUtilServerIoc
{
    public CommonServicesIoc Common {  get; set; }

    public BUtilServerIoc(ILog log, Action<string?> onGetLastMinuteMessage)
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
