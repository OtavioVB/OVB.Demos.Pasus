using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;
using static Grpc.Core.Metadata;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DataContext _dbContext;
    protected readonly ITraceManager _traceManager;

    protected BaseRepository(
        DataContext dbContext,
        ITraceManager traceManager)
    {
        _dbContext = dbContext;
        _traceManager = traceManager;
    }

    public virtual Task AddAsync(TEntity entity, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(BaseRepository<TEntity>)}.{nameof(AddAsync)}",
            activityKind: ActivityKind.Internal,
            input: entity,
            handler: (input, inputAuditableInfo, activity, cancellationToken)
                => _dbContext.Set<TEntity>().AddAsync(input, cancellationToken).AsTask(),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);

    public virtual Task AddRangeAsync(TEntity[] entities, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(BaseRepository<TEntity>)}.{nameof(AddRangeAsync)}",
            activityKind: ActivityKind.Internal,
            input: entities,
            handler: (input, inputAuditableInfo, activity, cancellationToken)
                => _dbContext.Set<TEntity>().AddRangeAsync(input, cancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);

    public Task UpdateAsync(TEntity entity, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(BaseRepository<TEntity>)}.{nameof(AddRangeAsync)}",
            activityKind: ActivityKind.Internal,
            input: entity,
            handler: (input, inputAuditableInfo, activity, cancellationToken)
                => Task.Run(() => _dbContext.Set<TEntity>().Update(input), cancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
}
