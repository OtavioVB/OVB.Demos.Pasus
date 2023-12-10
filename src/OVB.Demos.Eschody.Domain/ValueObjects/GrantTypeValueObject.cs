using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct GrantTypeValueObject
{
    public bool IsValid { get; }
    private string GrantType { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private GrantTypeValueObject(bool isValid, string grantType, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        GrantType = grantType;
        ProcessResult = processResult;
    }

    public static GrantTypeValueObject Build(string grantType)
    {
        var hasNotifications = false;
        var notifications = new List<Notification>(1);

        if (grantType != "client_credentials")
        {
            hasNotifications = true;
            notifications.Add(NotificationFacilitator.GrantTypeValid);
        }

        if (hasNotifications)
            return new GrantTypeValueObject(
                isValid: false,
                grantType: string.Empty,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray()));
        else
            return new GrantTypeValueObject(
                isValid: true,
                grantType: grantType,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult());
    }

    public string GetGrantType()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return GrantType;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
