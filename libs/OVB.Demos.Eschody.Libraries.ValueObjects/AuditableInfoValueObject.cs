using Olizia.Demos.Resale.ValueObjects.Exceptions;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;

namespace OVB.Demos.Eschody.Libraries.ValueObjects;

public readonly struct AuditableInfoValueObject
{
    public bool IsValid { get; }
    private Guid CorrelationId { get; }
    private string SourcePlatform { get; }
    private string ExecutionUser { get; }
    private DateTime RequestedAt { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private AuditableInfoValueObject(
        bool isValid, Guid correlationId, string sourcePlatform, string executionUser, DateTime requestedAt, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        CorrelationId = correlationId;
        SourcePlatform = sourcePlatform;
        ExecutionUser = executionUser;
        RequestedAt = requestedAt;
        ProcessResult = processResult;
    }

    private AuditableInfoValueObject(bool isValid)
    {
        IsValid = isValid;
        CorrelationId = Guid.Empty;
        SourcePlatform = string.Empty;
        ExecutionUser = string.Empty;
        RequestedAt = DateTime.MinValue;
    }

    public const int ExecutionUserMaxLength = 32;
    public const int ExecutionUserMinLength = 5;

    public const int SourcePlatformMaxLength = 32;
    public const int SourcePlatformMinLength = 5;

    public static AuditableInfoValueObject Build(
        Guid correlationId, string sourcePlatform, string executionUser, DateTime requestedAt)
    {
        if (correlationId == Guid.Empty)
            return new AuditableInfoValueObject(false);

        if (sourcePlatform == string.Empty || sourcePlatform.Length > SourcePlatformMaxLength || sourcePlatform.Length < SourcePlatformMinLength)
            return new AuditableInfoValueObject(false);

        if (executionUser == string.Empty || executionUser.Length > ExecutionUserMaxLength || executionUser.Length < ExecutionUserMinLength)
            return new AuditableInfoValueObject(false);

        if (requestedAt > DateTime.UtcNow)
            return new AuditableInfoValueObject(false);

        return new AuditableInfoValueObject(
            isValid: true,
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            requestedAt: requestedAt,
            processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult());
    }

    public Guid GetCorrelationId()
    {
        EschodyValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return CorrelationId;
    }

    public string GetSourcePlatform()
    {
        EschodyValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return SourcePlatform;
    }

    public string GetExecutionUser()
    {
        EschodyValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return ExecutionUser;
    }

    public DateTime GetRequestedAt()
    {
        EschodyValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return RequestedAt;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;

}
