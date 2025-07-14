using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace BUtil.Core.Misc;
internal static class NetworkHelper
{
    public static IEnumerable<NetworkInfo> GetMyLocalIps(TcpListener listener)
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

        return items;
    }
}
