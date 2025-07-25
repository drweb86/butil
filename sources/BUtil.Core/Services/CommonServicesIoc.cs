﻿using BUtil.Core.BUtilServer;
using BUtil.Core.FIleSender;
using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using System;

namespace BUtil.Core.Services;

public class CommonServicesIoc : IDisposable
{
    private readonly Action<string?> _onGetLastMinuteMessage;

    public readonly ILastMinuteMessageService LastMinuteMessageService;
    public readonly ICachedHashService CachedHashService = new CachedHashService();
    public readonly ILog Log;
    internal readonly IEncryptionService EncryptionService = new EncryptionService();
    internal readonly ICompressionService CompressionService = new CompressionService();
    internal readonly IBUtilServerClientProtocol BUtilServerClientProtocol;
    internal readonly IBUtilServerProtocol BUtilServerProtocol;

    public CommonServicesIoc(ILog log, Action<string?> onGetLastMinuteMessage)
    {
        Log = log;
        LastMinuteMessageService = new LastMinuteMessageService(log);
        _onGetLastMinuteMessage = onGetLastMinuteMessage;
        BUtilServerClientProtocol = new BUtilServerClientProtocol(EncryptionService);
        BUtilServerProtocol = new BUtilServerProtocol(CachedHashService, EncryptionService);
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        _onGetLastMinuteMessage(LastMinuteMessageService.GetLastMinuteMessages());
        CachedHashService.Dispose();
        LastMinuteMessageService.Dispose();
    }
}
