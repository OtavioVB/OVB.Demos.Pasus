using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;

public interface IExtensionTenantRepository
{
    public Task<Tenant?> GetTenantByClientIdAsync(Guid clientId, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
    public Task<bool> VerifyTenantExistsByCnpjAsync(string cnpj, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
}
