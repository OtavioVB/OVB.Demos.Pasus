using OpenTelemetry.Trace;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Facilitators;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Diagnostics;
using System.Xml.Linq;

namespace OVB.Demos.Eschody.Libraries.Observability.Trace;

public sealed class TraceManager : ITraceManager
{
    private readonly ActivitySource _activitySource;

    public TraceManager(ActivitySource activitySource)
    {
        _activitySource = activitySource;
    }

    public async Task ExecuteTraceAsync<TInput>(
        string traceName, ActivityKind activityKind, TInput input, 
        Func<TInput, AuditableInfoValueObject, Activity, CancellationToken, Task> handler, 
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken,
        KeyValuePair<string, string>[]? keyValuePairs = null)
    {
        using var activity = _activitySource.StartActivity(
            name: traceName,
            kind: activityKind);

        if (activity is null)
            throw PasusValueObjectException.ExceptionFromActivityNull;

        activity.Start();

        if (keyValuePairs is not null)
            foreach (var keyValuePair in keyValuePairs)
                activity.AddTag(keyValuePair.Key, keyValuePair.Value);

        activity.AddTag(
            key: ObservabilityFacilitator.CorrelationIdKey,
            value: auditableInfo.GetCorrelationId().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.SourcePlatformKey,
            value: auditableInfo.GetSourcePlatform().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.ExecutionUserKey,
            value: auditableInfo.GetExecutionUser().ToString());

        try
        {
            await handler(
                arg1: input,
                arg2: auditableInfo,
                arg3: activity,
                arg4: cancellationToken);
            activity.SetStatus(ActivityStatusCode.Ok);
            return;
        }
        catch (Exception ex)
        {
            activity.RecordException(ex);
            activity.SetStatus(ActivityStatusCode.Error);
            throw;
        }
    }

    public async Task<TOutput> ExecuteTraceAsync<TInput, TOutput>(
        string traceName, ActivityKind activityKind, TInput input,
        Func<TInput, AuditableInfoValueObject, Activity, CancellationToken, Task<TOutput>> handler,
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken,
        KeyValuePair<string, string>[]? keyValuePairs = null)
    {
        using var activity = _activitySource.StartActivity(
            name: traceName,
            kind: activityKind);

        if (activity is null)
            throw PasusValueObjectException.ExceptionFromActivityNull;

        activity.Start();

        activity.AddTag(
            key: ObservabilityFacilitator.CorrelationIdKey,
            value: auditableInfo.GetCorrelationId().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.SourcePlatformKey,
            value: auditableInfo.GetSourcePlatform().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.ExecutionUserKey,
            value: auditableInfo.GetExecutionUser().ToString());

        if (keyValuePairs is not null)
            foreach (var keyValuePair in keyValuePairs)
                activity.AddTag(keyValuePair.Key, keyValuePair.Value);

        try
        {
            var result = await handler(
                arg1: input,
                arg2: auditableInfo,
                arg3: activity,
                arg4: cancellationToken);
            activity.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception ex)
        {
            activity.RecordException(ex);
            activity.SetStatus(ActivityStatusCode.Error);
            throw;
        }
    }

    public async Task<TOutput> ExecuteTraceAsync<TOutput>(
        string traceName, ActivityKind activityKind, 
        Func<AuditableInfoValueObject, Activity, CancellationToken, Task<TOutput>> handler,
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken,
        KeyValuePair<string, string>[]? keyValuePairs = null)
    {
        using var activity = _activitySource.StartActivity(
            name: traceName,
            kind: activityKind);

        if (activity is null)
            throw PasusValueObjectException.ExceptionFromActivityNull;

        activity.Start();

        if (keyValuePairs is not null)
            foreach (var keyValuePair in keyValuePairs)
                activity.AddTag(keyValuePair.Key, keyValuePair.Value);

        activity.AddTag(
            key: ObservabilityFacilitator.CorrelationIdKey,
            value: auditableInfo.GetCorrelationId().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.SourcePlatformKey,
            value: auditableInfo.GetSourcePlatform().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.ExecutionUserKey,
            value: auditableInfo.GetExecutionUser().ToString());

        try
        {
            var result = await handler(
                arg1: auditableInfo,
                arg2: activity,
                arg3: cancellationToken);
            activity.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception ex)
        {
            activity.RecordException(ex);
            activity.SetStatus(ActivityStatusCode.Error);
            throw;
        }
    }

    public async Task ExecuteTraceAsync(
        string traceName, ActivityKind activityKind, 
        Func<AuditableInfoValueObject, Activity, CancellationToken, Task> handler, 
        AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken,
        KeyValuePair<string, string>[]? keyValuePairs = null)
    {
        using var activity = _activitySource.StartActivity(
            name: traceName,
            kind: activityKind);

        if (activity is null)
            throw PasusValueObjectException.ExceptionFromActivityNull;

        activity.Start();

        if (keyValuePairs is not null)
            foreach (var keyValuePair in keyValuePairs)
                activity.AddTag(keyValuePair.Key, keyValuePair.Value);

        activity.AddTag(
            key: ObservabilityFacilitator.CorrelationIdKey,
            value: auditableInfo.GetCorrelationId().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.SourcePlatformKey,
            value: auditableInfo.GetSourcePlatform().ToString());
        activity.AddTag(
            key: ObservabilityFacilitator.ExecutionUserKey,
            value: auditableInfo.GetExecutionUser().ToString());

        try
        {
            await handler(
                arg1: auditableInfo,
                arg2: activity,
                arg3: cancellationToken);
            activity.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            activity.RecordException(ex);
            activity.SetStatus(ActivityStatusCode.Error);
            throw;
        }
    }
}
