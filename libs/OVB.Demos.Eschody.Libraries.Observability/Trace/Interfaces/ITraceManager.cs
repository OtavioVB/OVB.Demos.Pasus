using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;

public interface ITraceManager
{
    public Task ExecuteTraceAsync<TInput>(
        string traceName, ActivityKind activityKind, TInput input,
        Func<TInput, AuditableInfoValueObject, Activity, CancellationToken, Task> handler,
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
    public Task<TOutput> ExecuteTraceAsync<TInput, TOutput>(
        string traceName, ActivityKind activityKind, TInput input,
        Func<TInput, AuditableInfoValueObject, Activity, CancellationToken, Task<TOutput>> handler,
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
    public Task<TOutput> ExecuteTraceAsync<TInput, TInput2, TOutput>(
        string traceName, ActivityKind activityKind, TInput input, TInput2 input2,
        Func<TInput, TInput2, AuditableInfoValueObject, Activity, CancellationToken, Task<TOutput>> handler,
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);

    public Task<TOutput> ExecuteTraceAsync<TOutput>(
        string traceName, ActivityKind activityKind,
        Func<AuditableInfoValueObject, Activity, CancellationToken, Task<TOutput>> handler,
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
}
