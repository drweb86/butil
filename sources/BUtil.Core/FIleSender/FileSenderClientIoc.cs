using BUtil.Core.Logs;
using BUtil.Core.Services;
using System;
using System.IO;
using System.Net.Sockets;

namespace BUtil.Core.FIleSender;

public class FileSenderClientIoc
{
    public CommonServicesIoc Common { get; }
    public FileSenderClientIoc(ILog log, string folder, string password, Action<string?> onGetLastMinuteMessage)
    {
        Common = new CommonServicesIoc(log, onGetLastMinuteMessage);
        FileSenderClientProtocol = new FileSenderProtocol(this, folder, password);
    }
    public IFileSenderClientProtocol FileSenderClientProtocol { get; }
    public TcpClient Client { get; set; } = null!;
    public TcpListener TcpListener { get; set; } = null!;
    public NetworkStream Stream { get; set; } = null!;
    public BinaryWriter Writer { get; set; } = null!;
    public BinaryReader Reader { get; set; } = null!;

    public void Dispose()
    {
        Writer?.Dispose();
        Reader?.Dispose();
        Stream?.Dispose();
        Client.Dispose();

        TcpListener?.Dispose();
        Common.Dispose();
    }
}
