using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;

public interface IBaseRepository<TEntity>
    where TEntity : class
{
    public Task AddAsync(TEntity entity, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
    public Task AddRangeAsync(TEntity[] entities, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
    public Task UpdateAsync(TEntity entity, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
}
