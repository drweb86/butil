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
    public WhereFileSenderTaskViewModel(string host, int port, string title, string iconUrl)
    {
        Title = title;
        IconSource = LoadFromResource(new Uri("avares://butil-ui" + iconUrl));
        Host = host;
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
    public static string Server_Field_Address => Resources.Server_Field_Address;
    public static string Server_Field_Port => Resources.Server_Field_Port;
    #endregion

    #region Host

    private string? _host;

    public string? Host
    {
        get
        {
            return _host;
        }
        set
        {
            if (value == _host)
                return;
            _host = value;
            OnPropertyChanged(nameof(Host));
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
