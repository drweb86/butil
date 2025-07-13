using BUtil.Core.Logs;
using BUtil.Core.Services;
using System;
using System.IO;

namespace BUtil.Core.FIleSender;
public class FileSenderIoc: CommonServicesIoc
{
    public FileSenderIoc(ILog log, string folder, string password, Action<string?> onGetLastMinuteMessage)
        : base(log, onGetLastMinuteMessage)
    {
        Model = new FileSenderModel(folder, password);
        FileSenderProtocol = new FileSenderProtocol(this);
    }
    public FileSenderModel Model { get; }
    public IFileSenderProtocol FileSenderProtocol { get; }
}

public class FileSenderModel
{
    public FileSenderModel(string folder, string password)
    {
        Folder = new DirectoryInfo(folder).FullName;
        Password = password;
    }
    public string Password { get; }
    public string Folder {  get; }
}
