using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Inputs;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Outputs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Interfaces;

public interface ITenantService
{
    public Task<ProcessResult<Notification, CreateTenantServiceResult>> CreateTenantServiceAsync(
        CreateTenantServiceInput input,
        AuditableInfoValueObject auditableInfo,
        CancellationToken cancellationToken);
    public Task<ProcessResult<Notification, OAuthTenantAuthenticationServiceResult>> OAuthTenantAuthenticationServiceAsync(
        OAuthTenantAuthenticationServiceInput input,
        AuditableInfoValueObject auditableInfo,
        CancellationToken cancellationToken);
}
