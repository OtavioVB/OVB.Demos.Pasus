using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Inputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Outputs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Interfaces;

public interface IOAuthTenantAuthenticationDomainFunction
{
    public Task<ProcessResult<Notification, OAuthTenantAuthenticationDomainFunctionResult>> OAuthTenantAuthenticationDomainFunctionAsync(
        OAuthTenantAuthenticationDomainFunctionInput input,
        AuditableInfoValueObject auditableInfo,
        Func<Guid, AuditableInfoValueObject, CancellationToken, Task<Tenant?>> getTenantFromClientId,
        CancellationToken cancellationToken);
}
