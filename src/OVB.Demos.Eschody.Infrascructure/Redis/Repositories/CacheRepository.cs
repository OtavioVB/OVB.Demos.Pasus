using Microsoft.Extensions.Caching.Distributed;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Facilitators;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Infrascructure.Redis.Repositories;

public sealed class CacheRepository : ICacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly ITraceManager _traceManager;

    public CacheRepository(
        IDistributedCache distributedCache,
        ITraceManager traceManager)
    {
        _distributedCache = distributedCache;
        _traceManager = traceManager;
    }

    private const int SlidingExpirationSeconds = 86400;
    private const int ExpirationSeconds = 604800;

    public Task<byte[]?> GetCacheAsync(string key, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(CacheRepository)}.{nameof(GetCacheAsync)}",
            activityKind: ActivityKind.Client,
            input: key,
            handler: (inputKey, inputAuditableInfo, activity, inputCancellationToken)
                => _distributedCache.GetAsync(inputKey, inputCancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);

    public Task SetCacheAsync(string key, byte[] value, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(CacheRepository)}.{nameof(SetCacheAsync)}",
            activityKind: ActivityKind.Internal,
            handler: (inputAuditableInfo, activity, inputCancellationToken)
                => _distributedCache.SetAsync(
                    key: key,
                    value: value,
                    options: new DistributedCacheEntryOptions()
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(SlidingExpirationSeconds),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ExpirationSeconds)
                    },
                    token: inputCancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
}
