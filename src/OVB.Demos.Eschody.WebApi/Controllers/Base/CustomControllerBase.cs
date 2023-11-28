using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base.Models;

namespace OVB.Demos.Eschody.WebApi.Controllers.Base;

public abstract class CustomControllerBase : ControllerBase
{
    public const string HttpExecutionUserTitle = "X-Execution-User";
    public const string HttpSourcePlatformTitle = "X-Source-Platform";
    public const string HttpCorrelationIdTitle = "X-Correlation-Id";
    public const string HttpRequestedAtTitle = "X-Requested-At";
    public const string HttpIdempotencyKeyTitle = "X-Idempotency-Key";

    public CustomUnprocessableEntityResult GetUnprocessableEntityForInvalidAuditable()
        => CustomUnprocessableEntityResult.Build(
            propertyName: nameof(AuditableInfoValueObject),
            propertyDescription: "The auditable info data is invalid.");

    public void AddAuditableInfoAtHeadersResponse(HttpResponse response, AuditableInfoValueObject auditableInfo, string idempotencyKey)
    {
        response.Headers.Append(HttpCorrelationIdTitle, auditableInfo.GetCorrelationId().ToString());
        response.Headers.Append(HttpRequestedAtTitle, auditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm:ss"));
        response.Headers.Append(HttpSourcePlatformTitle, auditableInfo.GetSourcePlatform());
        response.Headers.Append(HttpExecutionUserTitle, auditableInfo.GetExecutionUser());
        response.Headers.Append(HttpIdempotencyKeyTitle, idempotencyKey);
    }
}
