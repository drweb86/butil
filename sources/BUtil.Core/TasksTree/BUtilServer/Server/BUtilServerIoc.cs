using BUtil.Core.FIleSender;
using BUtil.Core.Logs;
using BUtil.Core.Services;
using System;
using System.Net.Sockets;

namespace BUtil.Core.TasksTree.BUtilServer.Server;
public class BUtilServerIoc
{
    public CommonServicesIoc Common {  get; set; }

    public BUtilServerIoc(ILog log, string folder, string password, Action<string?> onGetLastMinuteMessage)
    {
        Common = new CommonServicesIoc(log, onGetLastMinuteMessage);
    }

    public TcpListener TcpListener { get; set; } = null!;
    public void Dispose()
    {
        TcpListener?.Dispose();
        Common.Dispose();
    }
}
