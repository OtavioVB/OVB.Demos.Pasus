using Microsoft.EntityFrameworkCore;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork;

public sealed class DefaultUnitOfWork : IUnitOfWork
{
    private readonly DataContext _dbContext;

    public DefaultUnitOfWork(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task ApplyDatabaseTransactionAsync(CancellationToken cancellationToken)
        => _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<TOutput> ExecuteUnitOfWorkAsync<TOutput>(
        Func<CancellationToken, Task<(bool HasDone, TOutput Output)>> handler, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();

        var handlerResponse = await handler(cancellationToken);

        if (handlerResponse.HasDone == false)
        {
            await transaction.RollbackAsync(cancellationToken);
            await transaction.DisposeAsync();
            return handlerResponse.Output;
        }
        else
        {
            await transaction.CommitAsync(cancellationToken);
            await transaction.DisposeAsync();
            return handlerResponse.Output;
        }
    }
}
