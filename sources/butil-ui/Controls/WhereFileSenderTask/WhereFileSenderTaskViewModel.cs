using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BUtil.Core;
using BUtil.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace butil_ui.Controls;

public class WhereFileSenderTaskViewModel : ObservableObject
{
    public WhereFileSenderTaskViewModel(string ip, int port, string title, string iconUrl)
    {
        Title = title;
        IconSource = LoadFromResource(new Uri("avares://butil-ui" + iconUrl));
        IP = ip;
        Port = port;
    }

    private static Bitmap LoadFromResource(Uri resourceUri)
    {
        return new Bitmap(AssetLoader.Open(resourceUri));
    }
    public string Title { get; }
    public Bitmap? IconSource { get; }

    public List<string> FtpsEncryptionSource { get; } = [];

    #region Labels
    public static string LeftMenu_Where => Resources.LeftMenu_Where;
    public static string Server_Field_Address => "IP";
    public static string Server_Field_Port => Resources.Server_Field_Port;
    #endregion

    #region IP

    private string? _ip;

    public string? IP
    {
        get
        {
            return _ip;
        }
        set
        {
            if (value == _ip)
                return;
            _ip = value;
            OnPropertyChanged(nameof(IP));
        }
    }

    #endregion

    #region Port

    private int _port;

    public int Port
    {
        get
        {
            return _port;
        }
        set
        {
            if (value == _port)
                return;
            _port = value;
            OnPropertyChanged(nameof(Port));
        }
    }

    #endregion
}
