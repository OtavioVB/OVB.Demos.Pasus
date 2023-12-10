using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent.Outputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Inputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Outputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Inputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Outputs;
using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Metric.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Facilitators;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base;
using OVB.Demos.Eschody.WebApi.Controllers.TenantContext.Payloads;
using OVB.Demos.Eschody.WebApi.Controllers.TenantContext.Sendloads;
using System.Diagnostics;
using System.Net.Mime;

namespace OVB.Demos.Eschody.WebApi.Controllers.TenantContext;

[Route("api/v1/backoffice/tenant")]
public sealed class TenantController : CustomControllerBase
{
    public TenantController(ITraceManager traceManager, ICacheRepository cacheRepository, IMetricManager metricManager) 
        : base(traceManager, cacheRepository, metricManager)
    {
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Route("create")]
    [AllowAnonymous]
    public Task<IActionResult> HttpPostCreateTenantAsync(
        [FromServices] IUseCase<CreateTenantUseCaseInput, CreateTenantUseCaseResult> useCase,
        [FromServices] IAuthorizationManager authorizationManager,
        [FromHeader(Name = AuthorizationManager.CreateTenantAuthorizationKey)] string authorizationKey,
        [FromHeader(Name = AuditableInfoValueObject.IdempotencyHeaderKey)] string idempotencyKey,
        [FromHeader(Name = AuditableInfoValueObject.CorrelationIdKey)] Guid correlationId,
        [FromHeader(Name = AuditableInfoValueObject.SourcePlatformKey)] string sourcePlatform,
        [FromHeader(Name = AuditableInfoValueObject.ExecutionUserKey)] string executionUser,
        [FromBody] CreateTenantPayloadInput input,
        CancellationToken cancellationToken)
    {
        if (authorizationKey != authorizationManager.CreateTenantKey)
            return Task.FromResult((IActionResult)StatusCode(StatusCodes.Status401Unauthorized, null));

        var auditableInfo = AuditableInfoValueObject.Build(
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            idempotencyKey: idempotencyKey,
            requestedAt: DateTime.UtcNow);
        if (!auditableInfo.IsValid)
            return Task.FromResult((IActionResult)StatusCode(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                value: GetUnprocessableEntityForInvalidAuditable()));

        return _traceManager.ExecuteTraceAsync<CreateTenantPayloadInput, IActionResult>(
            traceName: nameof(HttpPostCreateTenantAsync),
            activityKind: ActivityKind.Internal,
            input: input,
            handler: async (input, auditableInfo, activity, cancellationToken) =>
            {
                var actionCacheKey = auditableInfo.GenerateCacheKeyWithIdempotencyKey(
                        cacheKey: nameof(HttpPostCreateTenantAsync));
                var statusCode = 503;
                var hasUsedIdempotencyCache = false;

                HttpContext.Response.Headers.Append(AuditableInfoValueObject.CorrelationIdKey, auditableInfo.GetCorrelationId().ToString());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.RequestedAtKey, auditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm:ss"));
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.SourcePlatformKey, auditableInfo.GetSourcePlatform());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.ExecutionUserKey, auditableInfo.GetExecutionUser());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.IdempotencyHeaderKey, auditableInfo.GetIdempotencyKey());

                _metricManager.CreateCounterIfNotExists(
                    counterName: nameof(HttpPostCreateTenantAsync));
                _metricManager.IncrementCounter(
                    counterName: nameof(HttpPostCreateTenantAsync),
                    auditableInfo: auditableInfo,
                    keyValuePairs: [
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: (object?)auditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: (object?)auditableInfo.GetSourcePlatform().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: (object?)auditableInfo.GetExecutionUser().ToString())
                    ]);

                var cache = await GetCacheFromIdempotencyKeyAsync(
                    actionCacheKey: actionCacheKey,
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);
                if (cache is not null)
                {
                    statusCode = cache.StatusCode;
                    hasUsedIdempotencyCache = true;

                    activity.AppendSpanTag(
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpMethodKey,
                            value: "HTTP POST"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.EndpointKey,
                            value: "api/v1/backoffice/tenant/create"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: auditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: auditableInfo.GetExecutionUser()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: auditableInfo.GetSourcePlatform()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.IdempotencyKey,
                            value: auditableInfo.GetIdempotencyKey()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.StatusCodeKey,
                            value: statusCode.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                            value: hasUsedIdempotencyCache.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteHostKey,
                            value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteAddrKey,
                            value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpForwardedForKey,
                            value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                    return StatusCode(
                        statusCode: cache.StatusCode,
                        value: cache.Response);
                }

                var useCaseResult = await useCase.ExecuteUseCaseAsync(
                    input: CreateTenantUseCaseInput.Build(
                        email: EmailValueObject.Build(input.Email),
                        password: PasswordValueObject.Build(input.Password, false),
                        comercialName: ComercialNameValueObject.Build(input.ComercialName),
                        socialReason: SocialReasonValueObject.Build(input.SocialReason),
                        primaryCnaeCode: CnaeCodeValueObject.Build(input.PrimaryCnaeCode),
                        cnpj: CnpjValueObject.Build(input.Cnpj),
                        composition: CompositionValueObject.Build(input.Composition),
                        scope: TenantScopeValueObject.Build(input.Scope),
                        foundationDate: FoundationDateValueObject.Build(input.FoundationDate)),
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);

                if (useCaseResult.IsSuccess)
                {
                    var sendload = new CreateTenantSendload(
                        clientId: useCaseResult.Output.Credentials.GetClientId(),
                        clientSecret: useCaseResult.Output.Credentials.GetClientSecret(),
                        email: useCaseResult.Output.Email.GetEmail(),
                        cnpj: useCaseResult.Output.Cnpj.GetCnpj(),
                        comercialName: useCaseResult.Output.ComercialName.GetComercialName(),
                        socialReason: useCaseResult.Output.SocialReason.GetSocialReason(),
                        primaryCnaeCode: useCaseResult.Output.PrimaryCnaeCode.GetCnaeCode(),
                        composition: (int)useCaseResult.Output.Composition.GetComposition(),
                        scope: useCaseResult.Output.Scope.GetScope(),
                        foundationDate: DateTime.SpecifyKind(useCaseResult.Output.FoundationDate.GetFoundationDate(), 
                            DateTimeKind.Unspecified).AddHours(-3).ToString("dd/MM/yyyy"),
                        isAvailableUntil: useCaseResult.Output.IsAvailableUntil,
                        isEnabled: useCaseResult.Output.IsEnabled);
                    statusCode = StatusCodes.Status201Created;
                    await SetCacheFromIdempotencyKeyAsync(
                        actionCacheKey: actionCacheKey,
                        statusCode: statusCode,
                        content: useCaseResult.Output,
                        auditableInfo: auditableInfo,
                        cancellationToken: cancellationToken);

                    activity.AppendSpanTag(
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpMethodKey,
                            value: "HTTP POST"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.EndpointKey,
                            value: "api/v1/backoffice/student/create"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: auditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: auditableInfo.GetExecutionUser()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: auditableInfo.GetSourcePlatform()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.IdempotencyKey,
                            value: auditableInfo.GetIdempotencyKey()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.StatusCodeKey,
                            value: statusCode.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                            value: hasUsedIdempotencyCache.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteHostKey,
                            value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteAddrKey,
                            value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpForwardedForKey,
                            value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                    return StatusCode(
                        statusCode: statusCode,
                        value: useCaseResult.Output);
                }

                if (useCaseResult.IsPartial)
                    throw new NotImplementedException();

                activity.AppendSpanTag(
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HttpMethodKey,
                        value: "HTTP POST"),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.EndpointKey,
                        value: "api/v1/backoffice/student/create"),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.CorrelationIdKey,
                        value: auditableInfo.GetCorrelationId().ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.ExecutionUserKey,
                        value: auditableInfo.GetExecutionUser()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.SourcePlatformKey,
                        value: auditableInfo.GetSourcePlatform()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.IdempotencyKey,
                        value: auditableInfo.GetIdempotencyKey()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.StatusCodeKey,
                        value: statusCode.ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                        value: hasUsedIdempotencyCache.ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.RemoteHostKey,
                        value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.RemoteAddrKey,
                        value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HttpForwardedForKey,
                        value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                statusCode = StatusCodes.Status400BadRequest;

                await SetCacheFromIdempotencyKeyAsync(
                    actionCacheKey: actionCacheKey,
                    statusCode: statusCode,
                    content: useCaseResult.Notifications,
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);

                return StatusCode(
                    statusCode: statusCode,
                    value: useCaseResult.Notifications);
            },
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Route("oauth/token")]
    [AllowAnonymous]
    public Task<IActionResult> HttpPostOAuthTenantAuthenticationAsync(
        [FromServices] IUseCase<OAuthTenantAuthenticationUseCaseInput, OAuthTenantAuthenticationUseCaseResult> useCase,
        [FromHeader(Name = AuditableInfoValueObject.IdempotencyHeaderKey)] string idempotencyKey,
        [FromHeader(Name = AuditableInfoValueObject.CorrelationIdKey)] Guid correlationId,
        [FromHeader(Name = AuditableInfoValueObject.SourcePlatformKey)] string sourcePlatform,
        [FromHeader(Name = AuditableInfoValueObject.ExecutionUserKey)] string executionUser,
        [FromHeader(Name = TenantCredentialsValueObject.ClientIdHeaderKey)] Guid clientId,
        [FromHeader(Name = TenantCredentialsValueObject.ClientSecretHeaderKey)] Guid clientSecret,
        [FromBody] OAuthTenantAuthenticationPayloadInput input,
        CancellationToken cancellationToken)
    {
        var auditableInfo = AuditableInfoValueObject.Build(
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            idempotencyKey: idempotencyKey,
            requestedAt: DateTime.UtcNow);
        if (!auditableInfo.IsValid)
            return Task.FromResult((IActionResult)StatusCode(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                value: GetUnprocessableEntityForInvalidAuditable()));

        return _traceManager.ExecuteTraceAsync<OAuthTenantAuthenticationPayloadInput, IActionResult>(
            traceName: nameof(HttpPostOAuthTenantAuthenticationAsync),
            activityKind: ActivityKind.Internal,
            input: input,
            handler: async (input, auditableInfo, activity, cancellationToken) =>
            {
                var actionCacheKey = auditableInfo.GenerateCacheKeyWithIdempotencyKey(
                        cacheKey: nameof(HttpPostOAuthTenantAuthenticationAsync));
                var statusCode = 503;
                var hasUsedIdempotencyCache = false;

                HttpContext.Response.Headers.Append(AuditableInfoValueObject.CorrelationIdKey, auditableInfo.GetCorrelationId().ToString());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.RequestedAtKey, auditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm:ss"));
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.SourcePlatformKey, auditableInfo.GetSourcePlatform());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.ExecutionUserKey, auditableInfo.GetExecutionUser());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.IdempotencyHeaderKey, auditableInfo.GetIdempotencyKey());

                _metricManager.CreateCounterIfNotExists(
                    counterName: nameof(HttpPostOAuthTenantAuthenticationAsync));
                _metricManager.IncrementCounter(
                    counterName: nameof(HttpPostOAuthTenantAuthenticationAsync),
                    auditableInfo: auditableInfo,
                    keyValuePairs: [
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: (object?)auditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: (object?)auditableInfo.GetSourcePlatform().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: (object?)auditableInfo.GetExecutionUser().ToString())
                    ]);

                var cache = await GetCacheFromIdempotencyKeyAsync(
                    actionCacheKey: actionCacheKey,
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);
                if (cache is not null)
                {
                    statusCode = cache.StatusCode;
                    hasUsedIdempotencyCache = true;

                    activity.AppendSpanTag(
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpMethodKey,
                            value: "HTTP POST"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.EndpointKey,
                            value: "api/v1/backoffice/tenant/oauth/token"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: auditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: auditableInfo.GetExecutionUser()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: auditableInfo.GetSourcePlatform()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.IdempotencyKey,
                            value: auditableInfo.GetIdempotencyKey()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.StatusCodeKey,
                            value: statusCode.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                            value: hasUsedIdempotencyCache.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteHostKey,
                            value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteAddrKey,
                            value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpForwardedForKey,
                            value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                    return StatusCode(
                        statusCode: cache.StatusCode,
                        value: cache.Response);
                }

                var useCaseResult = await useCase.ExecuteUseCaseAsync(
                    input: OAuthTenantAuthenticationUseCaseInput.Build(
                        scope: TenantScopeValueObject.Build(input.Scope),
                        credentials: TenantCredentialsValueObject.Build(
                            clientId: clientId,
                            clientSecret: clientSecret),
                        grantType: GrantTypeValueObject.Build(input.GrantType)),
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);

                if (useCaseResult.IsSuccess)
                {
                    var sendload = new OAuthTenantAuthenticationSendload(
                        accessToken: useCaseResult.Output.AccessToken,
                        type: useCaseResult.Output.Type,
                        grantType: useCaseResult.Output.GrantType.GetGrantType(),
                        scope: useCaseResult.Output.Scope.GetScope(),
                        expiresIn: useCaseResult.Output.ExpiresIn,
                        notifications: useCaseResult.Notifications);
                    statusCode = StatusCodes.Status201Created;
                    await SetCacheFromIdempotencyKeyAsync(
                        actionCacheKey: actionCacheKey,
                        statusCode: statusCode,
                        content: sendload,
                        auditableInfo: auditableInfo,
                        cancellationToken: cancellationToken);

                    activity.AppendSpanTag(
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpMethodKey,
                            value: "HTTP POST"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.EndpointKey,
                            value: "api/v1/backoffice/student/create"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: auditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: auditableInfo.GetExecutionUser()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: auditableInfo.GetSourcePlatform()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.IdempotencyKey,
                            value: auditableInfo.GetIdempotencyKey()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.StatusCodeKey,
                            value: statusCode.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                            value: hasUsedIdempotencyCache.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteHostKey,
                            value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteAddrKey,
                            value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpForwardedForKey,
                            value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                    return StatusCode(
                        statusCode: statusCode,
                        value: sendload);
                }

                if (useCaseResult.IsPartial)
                    throw new NotImplementedException();

                activity.AppendSpanTag(
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HttpMethodKey,
                        value: "HTTP POST"),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.EndpointKey,
                        value: "api/v1/backoffice/student/create"),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.CorrelationIdKey,
                        value: auditableInfo.GetCorrelationId().ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.ExecutionUserKey,
                        value: auditableInfo.GetExecutionUser()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.SourcePlatformKey,
                        value: auditableInfo.GetSourcePlatform()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.IdempotencyKey,
                        value: auditableInfo.GetIdempotencyKey()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.StatusCodeKey,
                        value: statusCode.ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                        value: hasUsedIdempotencyCache.ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.RemoteHostKey,
                        value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.RemoteAddrKey,
                        value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HttpForwardedForKey,
                        value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                statusCode = StatusCodes.Status400BadRequest;

                await SetCacheFromIdempotencyKeyAsync(
                    actionCacheKey: actionCacheKey,
                    statusCode: statusCode,
                    content: useCaseResult.Notifications,
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);

                return StatusCode(
                    statusCode: statusCode,
                    value: useCaseResult.Notifications);
            }, 
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
    }
}
