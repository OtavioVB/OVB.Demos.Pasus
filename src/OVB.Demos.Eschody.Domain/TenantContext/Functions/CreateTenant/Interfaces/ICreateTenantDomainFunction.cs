using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Inputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Outputs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Interfaces;

public interface ICreateTenantDomainFunction
{
    public Task<ProcessResult<Notification, CreateTenantDomainFunctionResult>> CreateTenantDomainFunctionAsync(
        CreateTenantDomainFunctionInput input,
        AuditableInfoValueObject auditableInfo,
        Func<string, AuditableInfoValueObject, CancellationToken, Task<bool>> verifyTenantExistsByCnpj,
        CancellationToken cancellationToken);
}
