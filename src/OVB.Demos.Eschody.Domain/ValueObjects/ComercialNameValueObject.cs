using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Globalization;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct ComercialNameValueObject
{
    public bool IsValid { get; }
    private string ComercialName { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    public const int MaxLength = 255;
    public const int MinLength = 5;

    private ComercialNameValueObject(bool isValid, string comercialName, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        ComercialName = comercialName;
        ProcessResult = processResult;
    }

    public static ComercialNameValueObject Build(string comercialName)
    {
        bool hasNotification = false;
        var notifications = new List<Notification>(2);

        if (comercialName.Length > MaxLength)
        {
            notifications.Add(NotificationFacilitator.ComercialNameMaxLength);
            hasNotification = true;
        }

        if (comercialName.Length < MinLength)
        {
            notifications.Add(NotificationFacilitator.ComercialNameMinLength);
            hasNotification = true;
        }

        if (hasNotification)
            return new ComercialNameValueObject(
                isValid: false,
                comercialName: string.Empty,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        else
            return new ComercialNameValueObject(
                isValid: true,
                comercialName: CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(comercialName),
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult());
    }

    public string GetComercialName()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return ComercialName;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
