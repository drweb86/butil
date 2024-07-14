using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using System;

namespace BUtil.Core.Services;

public class CommonServicesIoc : IDisposable
{
    private readonly Action<string?> _onGetLastMinuteMessage;

    public readonly ILastMinuteMessageService LastMinuteMessageService;
    public readonly ICashedHashStoreService CashedHashStoreService;
    public readonly IHashService HashService;
    public readonly ILog Log;

    public CommonServicesIoc(ILog log, Action<string?> onGetLastMinuteMessage)
    {
        Log = log;
        LastMinuteMessageService = new LastMinuteMessageService(log);
        CashedHashStoreService = new CashedHashStoreService();
        HashService = new CachedHashService(CashedHashStoreService);
        _onGetLastMinuteMessage = onGetLastMinuteMessage;
    }

    public void Dispose()
    {
        _onGetLastMinuteMessage(LastMinuteMessageService.GetLastMinuteMessages());
        HashService.Dispose();
        LastMinuteMessageService.Dispose();
    }
}
