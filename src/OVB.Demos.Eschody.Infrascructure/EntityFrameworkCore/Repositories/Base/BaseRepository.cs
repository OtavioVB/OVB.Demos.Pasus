using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DbContext _dbContext;

    protected BaseRepository(
        DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();

    public virtual Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
        => _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        => Task.Run(() => _dbContext.Set<TEntity>().Update(entity), cancellationToken);
}
