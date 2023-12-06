using Microsoft.Extensions.Caching.Distributed;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;

namespace OVB.Demos.Eschody.Infrascructure.Redis.Repositories;

public sealed class CacheRepository : ICacheRepository
{
    private readonly IDistributedCache _distributedCache;

    public CacheRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public Task<byte[]?> GetCacheAsync(string key, CancellationToken cancellationToken)
        => _distributedCache.GetAsync(key, cancellationToken);

    public Task SetCacheAsync(string key, byte[] value, int expirationSeconds, int memoryExpirationSeconds, CancellationToken cancellationToken)
        => _distributedCache.SetAsync(
            key: key,
            value: value,
            options: new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(memoryExpirationSeconds),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSeconds)
            },
            token: cancellationToken);
}
