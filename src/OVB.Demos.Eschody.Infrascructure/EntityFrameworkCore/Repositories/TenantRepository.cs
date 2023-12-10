using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories;

public sealed class TenantRepository : BaseRepository<Tenant>, IExtensionTenantRepository
{
    public TenantRepository(DataContext dbContext, ITraceManager traceManager) 
        : base(dbContext, traceManager)
    {
    }

    public Task<Tenant?> GetTenantByClientIdAsync(Guid clientId, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(TenantRepository)}.{nameof(GetTenantByClientIdAsync)}",
            activityKind: ActivityKind.Internal,
            input: clientId,
            handler: (clientId, auditableInfo, activity, cancellationToken) =>
                _dbContext.Set<Tenant>().AsNoTracking().Where(p => p.ClientId == clientId).FirstOrDefaultAsync(cancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);

    public Task<bool> VerifyTenantExistsByCnpjAsync(string cnpj, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(TenantRepository)}.{nameof(VerifyTenantExistsByCnpjAsync)}",
            activityKind: ActivityKind.Internal,
            input: cnpj,
            handler: (cnpj, auditableInfo, activity, cancellationToken) =>
                _dbContext.Set<Tenant>().Where(p => p.Cnpj == cnpj).AnyAsync(cancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
}
