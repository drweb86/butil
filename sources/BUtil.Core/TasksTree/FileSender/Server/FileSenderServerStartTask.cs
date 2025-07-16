using BUtil.Core.Events;
using BUtil.Core.FIleSender;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using BUtil.Core.ConfigurationFileModels.V2;
using System.Net.NetworkInformation;
using System.Text;

namespace BUtil.Core.TasksTree.FileSender;

internal class FileSenderServerStartTask : BuTaskV2
{
    private readonly FileSenderServerIoc _ioc;
    private readonly FileSenderServerModelOptionsV2 _options;

    public FileSenderServerStartTask(FileSenderServerIoc ioc, TaskEvents events, FileSenderServerModelOptionsV2 options) :
        base(ioc.Common.Log, events, $"Start server")
    {
        _ioc = ioc;
        _options = options;
    }

    protected override void ExecuteInternal()
    {
        LogDebug($"Starting to listen TCP port {_options.Port} for incoming connections.");
        _ioc.TcpListener = new TcpListener(IPAddress.IPv6Any, _options.Port);
        _ioc.TcpListener.Server.DualMode = true;
        _ioc.TcpListener.Start();

        LogServerIp(_ioc.TcpListener);
    }

    private void LogServerIp(TcpListener listener)
    {
        var ips = NetworkHelper.GetMyLocalIps(listener);

        var helpMessage = new StringBuilder("You can connect to server via following IPs:\n");
        var priorityItems = new NetworkInterfaceType[] { NetworkInterfaceType.Wireless80211 };
        var itemsOrdered = ips
            .Where(x => x.Addresses.Any())
            .OrderByDescending(item => priorityItems.Contains(item.InterfaceType))
            .ThenBy(item => item.Name + item.Description)
            .ToList();
        itemsOrdered.ForEach(x => x.Addresses = x.Addresses.OrderBy(y => y.AddressFamily).ToList());

        foreach (var item in itemsOrdered)
        {
            helpMessage.AppendLine($"- {item.Name} ({item.Description}): {string.Join(',', item.Addresses.Select(FormatAddress))}");
        }
        
        var fakeTask = new FunctionBuTaskV2<bool>(_ioc.Common.Log, Events, helpMessage.ToString(), () => true);
        Events.DuringExecutionTasksAdded(Id, [fakeTask]);
        fakeTask.Execute();
    }

    private static string FormatAddress(IPAddress address)
    {
        return address.AddressFamily == AddressFamily.InterNetworkV6
            ? $"[{address}]"
            : address.ToString();
    }

}
