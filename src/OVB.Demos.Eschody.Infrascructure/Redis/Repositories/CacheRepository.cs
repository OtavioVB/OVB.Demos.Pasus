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

    private const int SlidingExpirationSeconds = 86400;
    private const int ExpirationSeconds = 604800;

    public Task<byte[]?> GetCacheAsync(string key, CancellationToken cancellationToken)
        => _distributedCache.GetAsync(key, cancellationToken);

    public Task SetCacheAsync(string key, byte[] value, CancellationToken cancellationToken)
        => _distributedCache.SetAsync(
            key: key,
            value: value,
            options: new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(SlidingExpirationSeconds),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ExpirationSeconds)
            },
            token: cancellationToken);
}
