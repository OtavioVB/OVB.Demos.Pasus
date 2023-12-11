using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Models;
using OVB.Demos.Eschody.Libraries.Observability.Metric.Interfaces;
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
    protected readonly IMetricManager _metricManager;

    protected CustomControllerBase(
        ITraceManager traceManager,
        ICacheRepository cacheRepository,
        IMetricManager metricManager)
    {
        _traceManager = traceManager;
        _cacheRepository = cacheRepository;
        _metricManager = metricManager;
    }

    public IActionResult StatusCodeMiddleware(
        int statusCode, object? result, Activity activity, bool hasUsedIdempotencyCache)
    {
        activity.AppendSpanTag(
            KeyValuePair.Create(
                key: ObservabilityFacilitator.StatusCodeKey,
                value: statusCode.ToString()),
            KeyValuePair.Create(
                key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                value: hasUsedIdempotencyCache.ToString()));

        return StatusCode(
            statusCode: statusCode,
            value: result);
    }

    public bool VerifyAuthenticationIsAuthorizationToScope(string endpointScope)
    {
        var authorizationScopes = HttpContext.User.FindFirst(TenantScopeValueObject.AuthorizationScopeKey)!.Value.Split(' ');
        var hasAccess = false;

        foreach (var uniqueScope in authorizationScopes)
        {
            if (uniqueScope == endpointScope)
            {
                hasAccess = true;
                break;
            }
        }

        return hasAccess;
    }

    public async Task SetCacheFromIdempotencyKeyAsync(string actionCacheKey, int statusCode, object? content, AuditableInfoValueObject auditableInfo,
        CancellationToken cancellationToken)
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
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
    }

    public async Task<CacheRequestModel?> GetCacheFromIdempotencyKeyAsync(string actionCacheKey, AuditableInfoValueObject auditableInfo,
        CancellationToken cancellationToken)
    {
        var cache = await _cacheRepository.GetCacheAsync(
                    key: actionCacheKey,
                    auditableInfo: auditableInfo,
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
            propertyDescription: "As informações de auditabilidade são inválidas.");

    public CustomUnprocessableEntityResult GetUnauthorizedEntityForInvalidScope()
        => CustomUnprocessableEntityResult.Build(
            propertyName: nameof(TenantScopeValueObject),
            propertyDescription: "O escopo da chave de autenticação não tange esse endpoint.");
}
