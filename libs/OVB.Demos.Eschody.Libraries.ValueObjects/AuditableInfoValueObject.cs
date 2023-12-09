using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Libraries.ValueObjects;

public readonly struct AuditableInfoValueObject
{
    public bool IsValid { get; }
    private Guid CorrelationId { get; }
    private string SourcePlatform { get; }
    private string ExecutionUser { get; }
    private DateTime RequestedAt { get; }
    private string IdempotencyKey { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private AuditableInfoValueObject(
        bool isValid, Guid correlationId, string sourcePlatform, string executionUser, DateTime requestedAt, string idempotencyKey, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        CorrelationId = correlationId;
        SourcePlatform = sourcePlatform;
        ExecutionUser = executionUser;
        RequestedAt = requestedAt;
        IdempotencyKey = idempotencyKey;
        ProcessResult = processResult;
    }

    private AuditableInfoValueObject(bool isValid)
    {
        IsValid = isValid;
        CorrelationId = Guid.Empty;
        SourcePlatform = string.Empty;
        ExecutionUser = string.Empty;
        IdempotencyKey = string.Empty;
        RequestedAt = DateTime.MinValue;
    }

    public const string CorrelationIdKey = "X-Correlation-Id";
    public const string SourcePlatformKey = "X-Source-Platform";
    public const string ExecutionUserKey = "X-Execution-User";
    public const string RequestedAtKey = "X-Requested-At";
    public const string IdempotencyHeaderKey = "X-Idempotency-Key";

    public const int ExecutionUserMaxLength = 32;
    public const int ExecutionUserMinLength = 5;

    public const int SourcePlatformMaxLength = 32;
    public const int SourcePlatformMinLength = 5;

    public const int IdempotencyKeyMaxLength = 32;
    public const int IdempotencyKeyMinLength = 5;

    public static AuditableInfoValueObject Build(
        Guid correlationId, string sourcePlatform, string executionUser, DateTime requestedAt, string idempotencyKey)
    {
        if (correlationId == Guid.Empty)
            return new AuditableInfoValueObject(false);

        if (sourcePlatform == string.Empty || sourcePlatform.Length > SourcePlatformMaxLength || sourcePlatform.Length < SourcePlatformMinLength)
            return new AuditableInfoValueObject(false);

        if (executionUser == string.Empty || executionUser.Length > ExecutionUserMaxLength || executionUser.Length < ExecutionUserMinLength)
            return new AuditableInfoValueObject(false);

        if (idempotencyKey == string.Empty || idempotencyKey.Length > IdempotencyKeyMaxLength || idempotencyKey.Length < IdempotencyKeyMinLength)
            return new AuditableInfoValueObject();

        if (requestedAt > DateTime.UtcNow)
            return new AuditableInfoValueObject(false);

        return new AuditableInfoValueObject(
            isValid: true,
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            requestedAt: requestedAt,
            idempotencyKey: idempotencyKey,
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

    public string GetIdempotencyKey()
    {
        EschodyValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return IdempotencyKey;
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

    public string GenerateCacheKeyWithIdempotencyKey(string cacheKey)
        => $"{cacheKey}.{GetIdempotencyKey()}";

}
