using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Models;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Facilitators;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base.Models;
using System.Diagnostics;
using System.Text.Json;

namespace OVB.Demos.Eschody.WebApi.Controllers.Base;

public abstract class CustomControllerBase : ControllerBase
{
    protected readonly ITraceManager _traceManager;
    protected readonly ICacheRepository _cacheRepository;

    protected CustomControllerBase(
        ITraceManager traceManager,
        ICacheRepository cacheRepository)
    {
        _traceManager = traceManager;
        _cacheRepository = cacheRepository;
    }

    public async Task SetCacheFromIdempotencyKeyAsync(string actionCacheKey, int statusCode, object? content, CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(
            utf8Json: memoryStream,
            value: new CacheRequestModel(
                statusCode: statusCode,
                response: content),
            cancellationToken: cancellationToken);
        await _cacheRepository.SetCacheAsync(
            key: actionCacheKey,
            value: memoryStream.ToArray(),
            cancellationToken: cancellationToken);
    }

    public async Task<CacheRequestModel?> GetCacheFromIdempotencyKeyAsync(string actionCacheKey, CancellationToken cancellationToken)
    {
        var cache = await _cacheRepository.GetCacheAsync(
                    key: actionCacheKey,
                    cancellationToken:cancellationToken);

        if (cache is null)
            return null;

        var memoryStream = new MemoryStream(cache);
        var cacheModel = await JsonSerializer.DeserializeAsync<CacheRequestModel>(
            utf8Json: memoryStream,
            cancellationToken: cancellationToken);
        return cacheModel;
    }

    public CustomUnprocessableEntityResult GetUnprocessableEntityForInvalidAuditable()
        => CustomUnprocessableEntityResult.Build(
            propertyName: nameof(AuditableInfoValueObject),
            propertyDescription: "The auditable info data is invalid.");

    protected static class TraceSpan
    {
        public static string EndpointKey = "Endpoint";
        public static string HttpMethodKey = "HttpMethod";
        public static string CorrelationIdKey = "CorrelationId";
        public static string ExecutionUserKey = "ExecutionUser";
        public static string SourcePlatformKey = "SourcePlatform";
        public static string StatusCodeKey = "StatusCode";
        public static string IdempotencyKey = "IdempotencyKey";
        public static string HasUsedIdempotencyCache = "HasUsedIdempotencyCache";
        public static string RemoteHostKey = "RemoteHost";
        public static string RemoteAddrKey = "RemoteAddress";
        public static string HttpForwardedForKey = "HttpForwardedFor";
    }
}
