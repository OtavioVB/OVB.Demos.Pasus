namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    public Task ApplyDatabaseTransactionAsync(CancellationToken cancellationToken);
    public Task<TOutput> ExecuteUnitOfWorkAsync<TOutput>(Func<CancellationToken, Task<(bool HasDone, TOutput Output)>> handler, CancellationToken cancellationToken);
}
