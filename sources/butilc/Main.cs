using BUtil.Core;
using BUtil.Core.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Linq;
using BUtil.Core.FileSystem;
using System.Diagnostics;
using BUtil.Core.FIleSender;
using butilc;
using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;
PlatformSpecificExperience.Instance.OsSleepPreventionService.PreventSleep();

Console.WriteLine(CopyrightInfo.Copyright);
Console.WriteLine();

try
{
    if (args != null && args.Length > 0 && (args[0] == "send" || args[0] == "receive"))
    {
        await DoTransfer(args);
        return;
    }

    new Controller()
        .ParseCommandLineArguments(args)
        .Launch();
}
catch (Exception e)
{
    ImproveIt.ProcessUnhandledException(e);
    Environment.Exit(-1);
}

static async Task DoTransfer(string[] args)
{
    if (args.Length < 1)
    {
        PrintUsage();
        return;
    }
    if (args[0] == "send")
    {
        if (args.Length != 5)
        {
            PrintUsage();
            return;
        }
        string sourceFolder = args[1];
        string ip = args[2];
        int port = int.Parse(args[3]);
        string password = args[4];
        await SendFolderAsync(sourceFolder, ip, port, password);
    }
    else if (args[0] == "receive")
    {
        if (args.Length != 4)
        {
            PrintUsage();
            return;
        }
        string outputFolder = args[1];
        int port = int.Parse(args[2]);
        string password = args[3];
        await ReceiveFolderAsync(outputFolder, port, password);
    }
    else
    {
        PrintUsage();
    }
}

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  Sending: butilc send \"[sourceFolder]\" [receiverIP] [receiverPort] \"password\"");
    Console.WriteLine("  Receiving: butilc receive \"[outputFolder]\" [listenPort] \"password\"");
}

static async Task SendFolderAsync(string sourceFolder, string ip, int port, string password)
{
    if (!Directory.Exists(sourceFolder))
    {
        throw new DirectoryNotFoundException($"Source folder not found: {sourceFolder}");
    }
    using var ioc = new FileSenderIoc(new NormalConsoleLog(), sourceFolder, password, x => { });
    var sourceItemState = ioc.FileSenderProtocol.TemporaryMoveMe();

    using (TcpClient client = new TcpClient())
    {
        Console.WriteLine($"Connecting to {ip}:{port}...");
        await client.ConnectAsync(ip, port);
        Console.WriteLine("Connected. Starting transfer...");

        using (NetworkStream stream = client.GetStream())
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true))
        {
            ioc.FileSenderProtocol.WriteProtocolVersion(writer);

            foreach (var fileState in sourceItemState.FileStates)
            {
                string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(sourceFolder, fileState);
                ioc.Log.WriteLine(BUtil.Core.Logs.LoggingEvent.Debug, $"{relativeFileName}");
                
                ioc.FileSenderProtocol.WriteCommandForServer(writer, FileTransferProtocolServerCommand.ReceiveFile);
                ioc.FileSenderProtocol.WriteFileHeader(writer, fileState);

                var clientCommand = ioc.FileSenderProtocol.ReadCommandForClient(stream);
                switch (clientCommand)
                {
                    case FileTransferProtocolClientCommand.Cancel:
                        ioc.Log.WriteLine(BUtil.Core.Logs.LoggingEvent.Debug, "File transfer is skipped.");
                        continue;
                    case FileTransferProtocolClientCommand.Continue:
                        await ioc.FileSenderProtocol.WriteFile(writer, fileState);
                        break;

                }
            }

            ioc.FileSenderProtocol.WriteCommandForServer(writer, FileTransferProtocolServerCommand.Disconnect);
        }
    }

    Console.WriteLine("Folder transfer completed successfully.");
}

static async Task ReceiveFolderAsync(string outputFolder, int port, string password)
{
    TcpListener listener = new TcpListener(IPAddress.IPv6Any, port);
    listener.Server.DualMode = true;
    listener.Start();

    using var ioc = new FileSenderIoc(new NormalConsoleLog(), outputFolder, password, x => { });
    ioc.Log.WriteLine(BUtil.Core.Logs.LoggingEvent.Debug, $"Waiting for folder to be received in directory: {ioc.Model.Folder}\n");
    Directory.CreateDirectory(ioc.Model.Folder);
    var sourceItemState = ioc.FileSenderProtocol.TemporaryMoveMe();

    PrintListeningAddresses(listener, password);

    using (TcpClient client = await listener.AcceptTcpClientAsync())
    {
        ioc.Log.WriteLine(BUtil.Core.Logs.LoggingEvent.Debug, "Client connected. Receiving folder...");

        using (NetworkStream stream = client.GetStream())
        using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
        {
            ioc.FileSenderProtocol.ReadCheckProtocolVersion(reader);

            while (true)
            {
                var command = ioc.FileSenderProtocol.ReadCommandForServer(reader);
                if (command == FileTransferProtocolServerCommand.ReceiveFile)
                {
                    var fileState = ioc.FileSenderProtocol.ReadFileHeader(reader);
                    Directory.CreateDirectory(Path.GetDirectoryName(fileState.FileName)!);

                    string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(ioc.Model.Folder, fileState);
                    ioc.Log.WriteLine(BUtil.Core.Logs.LoggingEvent.Debug, relativeFileName);

                    var existingFileState = sourceItemState.FileStates.SingleOrDefault(s => s.FileName == fileState.FileName);
                    if (existingFileState != null && !existingFileState.CompareTo(fileState, true))
                    {
                        ioc.Log.WriteLine(BUtil.Core.Logs.LoggingEvent.Debug, "File already exists and have same size, SHA-512 hash and last write time. Skipped...");
                        ioc.FileSenderProtocol.WriteCommandForClient(stream, FileTransferProtocolClientCommand.Cancel);
                    }
                    else
                    {
                        ioc.FileSenderProtocol.WriteCommandForClient(stream, FileTransferProtocolClientCommand.Continue);
                        await ioc.FileSenderProtocol.ReadFile(reader, fileState);
                    }
                }
                else if (command == FileTransferProtocolServerCommand.Disconnect)
                {
                    ioc.Log.WriteLine(BUtil.Core.Logs.LoggingEvent.Debug, "Client disconnected.");
                    break;
                }
            }
        }
    }
    listener.Stop();
}

static void PrintListeningAddresses(TcpListener listener, string password)
{
    IPEndPoint localEndpoint = (IPEndPoint)listener.LocalEndpoint;
    int port = localEndpoint.Port;

    var items = new List<NetworkInfo>();
    foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
    {
        if (netInterface.OperationalStatus == OperationalStatus.Down)
            continue;

        var item = new NetworkInfo(netInterface);
        items.Add(item);

        IPInterfaceProperties ipProps = netInterface.GetIPProperties();
        foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
            item.Addresses.Add(addr.Address);
    }

    Console.WriteLine("Use the next command to send folder depending on from which network you are connecting:\n");
    var priorityItems = new NetworkInterfaceType[] { NetworkInterfaceType.Wireless80211 };
    var itemsOrdered = items
        .Where(x => x.Addresses.Any())
        .OrderByDescending(item => priorityItems.Contains(item.InterfaceType))
        .ThenBy(item => item.Name + item.Description)
        .ToList();
    itemsOrdered.ForEach(x => x.Addresses = x.Addresses.OrderBy(y => y.AddressFamily).ToList());

    foreach (var item in itemsOrdered)
    {
        Console.WriteLine($"- {item.Name} ({item.Description}):");
        foreach (var addr in item.Addresses)
        {
            Console.WriteLine($"butilc send \"folder to transfer\" {FormatAddress(addr)} {port} \"{password}\"");
        }
    }
    Console.WriteLine();
}

static string FormatAddress(IPAddress address)
{
    return address.AddressFamily == AddressFamily.InterNetworkV6
        ? $"[{address}]"
        : address.ToString();
}

class NetworkInfo
{
    public NetworkInfo(NetworkInterface networkInterface)
    {
        Name = networkInterface.Name;
        Description = networkInterface.Description;
        InterfaceType = networkInterface.NetworkInterfaceType;
        Addresses = new List<IPAddress>();
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public NetworkInterfaceType InterfaceType { get; set; }

    public List<IPAddress> Addresses { get; set; }
}
