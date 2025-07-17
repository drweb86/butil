using BUtil.Core.Events;
using BUtil.Core.Misc;
using BUtil.Core.TasksTree.Core;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using BUtil.Core.ConfigurationFileModels.V2;
using System.Net.NetworkInformation;
using System.Text;
using BUtil.Core.Localization;
using System.Collections.Generic;

namespace BUtil.Core.TasksTree.BUtilServer.Server;

internal class BUtilServerStartTask : BuTaskV2
{
    private readonly BUtilServerIoc _ioc;
    private readonly BUtilServerModelOptionsV2 _options;

    public BUtilServerStartTask(BUtilServerIoc ioc, TaskEvents events, BUtilServerModelOptionsV2 options) :
        base(ioc.Common.Log, events, string.Format(Resources.BUtilServerStartTask_Name, options.Port))
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
        var fakeTasksForUi = new List<BuTask>();
        fakeTasksForUi.Add(new FunctionBuTaskV2<bool>(_ioc.Common.Log, Events, "IP:", () => true));

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
            fakeTasksForUi.Add(new FunctionBuTaskV2<bool>(_ioc.Common.Log, Events, $"- {item.Name} ({item.Description}): {string.Join(',', item.Addresses.Select(FormatAddress))}", () => true));
        }

        LogDebug(helpMessage.ToString());

        Events.DuringExecutionTasksAdded(Id, fakeTasksForUi);
        fakeTasksForUi.ForEach(x => x.Execute());
    }

    private static string FormatAddress(IPAddress address)
    {
        return address.AddressFamily == AddressFamily.InterNetworkV6
            ? $"[{address}]"
            : address.ToString();
    }

}
