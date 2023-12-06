using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base.Models;

namespace OVB.Demos.Eschody.WebApi.Controllers.Base;

public abstract class CustomControllerBase : ControllerBase
{
    public CustomUnprocessableEntityResult GetUnprocessableEntityForInvalidAuditable()
        => CustomUnprocessableEntityResult.Build(
            propertyName: nameof(AuditableInfoValueObject),
            propertyDescription: "The auditable info data is invalid.");

    public void AddAuditableInfoAtHeadersResponse(HttpResponse response, AuditableInfoValueObject auditableInfo, string idempotencyKey)
    {
        response.Headers.Append(AuditableInfoValueObject.CorrelationIdKey, auditableInfo.GetCorrelationId().ToString());
        response.Headers.Append(AuditableInfoValueObject.RequestedAtKey, auditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm:ss"));
        response.Headers.Append(AuditableInfoValueObject.SourcePlatformKey, auditableInfo.GetSourcePlatform());
        response.Headers.Append(AuditableInfoValueObject.ExecutionUserKey, auditableInfo.GetExecutionUser());
        response.Headers.Append(AuditableInfoValueObject.IdempotencyKey, idempotencyKey);
    }
}
