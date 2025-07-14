using BUtil.Core.Logs;
using BUtil.Core.Services;
using System;
using System.IO;
using System.Net.Sockets;

namespace BUtil.Core.FIleSender;
public class FileSenderServerIoc
{
    public CommonServicesIoc Common {  get; set; }

    public FileSenderServerIoc(ILog log, string folder, string password, Action<string?> onGetLastMinuteMessage)
    {
        Common = new CommonServicesIoc(log, onGetLastMinuteMessage);
        FileSenderServerProtocol = new FileSenderServerProtocol(this, folder, password);
    }

    public IFileSenderServerProtocol FileSenderServerProtocol { get; }
    public TcpClient Client { get; set; } = null!;
    public TcpListener TcpListener { get; set; } = null!;
    public NetworkStream Stream { get; set; } = null!;
    public BinaryReader Reader { get; set; } = null!;

    public void Dispose()
    {
        Reader?.Dispose();
        Stream?.Dispose();
        Client.Dispose();

        TcpListener?.Dispose();
        Common.Dispose();
    }
}
