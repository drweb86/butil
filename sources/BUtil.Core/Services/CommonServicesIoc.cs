using BUtil.Core.Hashing;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using System;

namespace BUtil.Core.Services;

public class CommonServicesIoc : IDisposable
{
    public ICashedHashStoreService CashedHashStoreService { get; }
    public IHashService HashService { get; }
    public ILog Log { get; }

    public CommonServicesIoc(ILog log)
    {
        Log = log;
        CashedHashStoreService = new CashedHashStoreService();
        HashService = new CachedHashService(CashedHashStoreService);
    }

    public void Dispose()
    {
        HashService.Dispose();
    }
}
