using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Domain.TenantContext.ENUMs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct CompositionValueObject
{
    public bool IsValid { get; }
    private TypeTenantComposition Composition { get; }
    private ProcessResult<Notification> ProcessResult { get; }
    
    private CompositionValueObject(bool isValid, TypeTenantComposition composition, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Composition = composition;
        ProcessResult = processResult;
    }

    public static CompositionValueObject Build(int composition)
    {
        bool hasNotifications = false;
        var notifications = new List<Notification>(1);
        var enumComposition = (TypeTenantComposition)composition;

        if (!Enum.IsDefined(enumComposition))
        {
            hasNotifications = true;
            notifications.Add(NotificationFacilitator.CompositionNeedsToBeValid);
        }

        if (!hasNotifications)
            return new CompositionValueObject(
                isValid: true,
                composition: enumComposition,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: null,
                    exceptions: null));
        else
            return new CompositionValueObject(
                isValid: false,
                composition: enumComposition,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
    }

    public TypeTenantComposition GetComposition()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Composition;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
