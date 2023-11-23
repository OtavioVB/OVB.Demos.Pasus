namespace OVB.Demos.Eschody.Libraries.ValueObjects;

public readonly struct AuditableInfoValueObject
{
    public AuditableInfoValueObject(Guid correlationId, string sourcePlatform, string executionUser)
    {
        CorrelationId = correlationId;
        SourcePlatform = sourcePlatform;
        ExecutionUser = executionUser;
    }

    public Guid CorrelationId { get; }
    public string SourcePlatform { get; }
    public string ExecutionUser { get; }
}
