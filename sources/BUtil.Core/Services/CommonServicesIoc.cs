using BUtil.Core.Hashing;
using BUtil.Core.Options;
using System;

namespace BUtil.Core.Services;

public class CommonServicesIoc : IDisposable
{
    public ICashedHashStoreService CashedHashStoreService { get; }
    public IHashService HashService { get; }

    public CommonServicesIoc()
    {
        CashedHashStoreService = new CashedHashStoreService();
        HashService = new CachedHashService(CashedHashStoreService);
    }

    public void Dispose()
    {
        HashService.Dispose();
    }
}
