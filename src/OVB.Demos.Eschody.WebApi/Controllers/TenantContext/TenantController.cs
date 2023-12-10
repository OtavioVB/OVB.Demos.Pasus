using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Metric.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base;
using OVB.Demos.Eschody.WebApi.Controllers.TenantContext.Payloads;
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
    [Route("oauth/token")]
    [AllowAnonymous]
    public Task<IActionResult> OAuthTenantAuthenticationAsync(
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

        throw new NotImplementedException();
    }
}
