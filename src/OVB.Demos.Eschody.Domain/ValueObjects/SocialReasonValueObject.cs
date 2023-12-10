using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Globalization;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct SocialReasonValueObject
{
    public bool IsValid { get; }
    private string SocialReason { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private SocialReasonValueObject(bool isValid, string socialReason, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        SocialReason = socialReason;
        ProcessResult = processResult;
    }

    public const int MaxLength = 255;
    public const int MinLength = 5;

    public static SocialReasonValueObject Build(string socialReason)
    {
        bool hasNotification = false;
        var notifications = new List<Notification>(2);

        if (socialReason.Length > MaxLength)
            notifications.Add(NotificationFacilitator.SocialReasonMaxLength);

        if (socialReason.Length < MinLength)
            notifications.Add(NotificationFacilitator.SocialReasonMinLength);

        if (hasNotification == true)
            return new SocialReasonValueObject(
                isValid: false,
                socialReason: string.Empty,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        else
            return new SocialReasonValueObject(
                isValid: true,
                socialReason: CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(socialReason).ToUpper(),
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult());
    }

    public string GetSocialReason()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

         return SocialReason;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
