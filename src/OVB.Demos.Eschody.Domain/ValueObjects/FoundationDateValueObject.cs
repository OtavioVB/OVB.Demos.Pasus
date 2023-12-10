using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct FoundationDateValueObject
{
    public bool IsValid { get; }
    private DateTime FoundationDate { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private FoundationDateValueObject(bool isValid, DateTime foundationDate, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        FoundationDate = foundationDate;
        ProcessResult = processResult;
    }

    public static FoundationDateValueObject Build(DateTime foundationDate)
    {
        var hasNotifications = false;
        var notifications = new List<Notification>(1);

        var newFoundationDate = DateTime.SpecifyKind(foundationDate, DateTimeKind.Utc);

        if (newFoundationDate.Date > DateTime.UtcNow.Date)
        {
            hasNotifications = true;
            notifications.Add(NotificationFacilitator.FoundationDateLessThanActualTime);
        }

        if (hasNotifications)
            return new FoundationDateValueObject(
                isValid: false,
                foundationDate: DateTime.MinValue,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray()));
        else
            return new FoundationDateValueObject(
               isValid: true,
               foundationDate: newFoundationDate,
               processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult());
    }

    public DateTime GetFoundationDate()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return FoundationDate;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
