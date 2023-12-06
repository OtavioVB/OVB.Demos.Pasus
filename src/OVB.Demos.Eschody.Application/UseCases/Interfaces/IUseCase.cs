using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Application.UseCases.Interfaces;

public interface IUseCase<TInput, TOutput>
{
    public Task<ProcessResult<Notification, TOutput>> ExecuteUseCaseAsync(
        TInput input, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
}
