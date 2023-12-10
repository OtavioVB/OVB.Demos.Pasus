using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct TenantCredentialsValueObject
{
    public bool IsValid { get; }
    private Guid ClientId { get; }
    private Guid ClientSecret { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private TenantCredentialsValueObject(bool isValid, Guid clientId, Guid clientSecret, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        ClientId = clientId;
        ClientSecret = clientSecret;
        ProcessResult = processResult;
    }

    public const string ClientIdHeaderKey = "X-Client-Id";
    public const string ClientSecretHeaderKey = "X-Client-Secret";

    public static TenantCredentialsValueObject Build(Guid clientId, Guid clientSecret)
    {
        var notifications = new List<Notification>(1);

        if (Guid.Empty == clientId || Guid.Empty == clientSecret)
        {
            notifications.Add(NotificationFacilitator.CredentialsNeedsToBeValid);
            return new TenantCredentialsValueObject(
                isValid: false,
                clientId: clientId,
                clientSecret: clientSecret,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
        else
        {
            return new TenantCredentialsValueObject(
                isValid: true,
                clientId: clientId,
                clientSecret: clientSecret,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: null,
                    exceptions: null));
        }
    }

    public static TenantCredentialsValueObject Build()
        => new TenantCredentialsValueObject(
            isValid: true,
            clientId: Guid.NewGuid(),
            clientSecret: Guid.NewGuid(),
            processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                notifications: null,
                exceptions: null));

    public Guid GetClientId()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);
        
        return ClientId;
    }

    public Guid GetClientSecret()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return ClientSecret;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
