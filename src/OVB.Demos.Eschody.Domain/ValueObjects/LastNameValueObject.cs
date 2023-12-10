using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Globalization;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct LastNameValueObject
{
    public bool IsValid { get; }
    public string LastName { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private LastNameValueObject(
        bool isValid, string lastName, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        LastName = lastName;
        ProcessResult = processResult;
    }

    private LastNameValueObject(bool isValid, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        LastName = string.Empty;
        ProcessResult = processResult;
    }

    public const int LastNameMaxLength = 32;
    public const int LastNameMinLength = 2;

    public static LastNameValueObject Build(string lastName)
    {
        var notifications = new List<Notification>();

        if (lastName.Length > LastNameMaxLength)
            notifications.Add(
                item: NotificationFacilitator.LastNameMaxLength);

        if (lastName.Length < LastNameMinLength)
            notifications.Add(
                item: NotificationFacilitator.LastNameMinLength);

        var cultureInfo = CultureInfo.GetCultureInfo("pt-BR");
        var lastNameTitleCase = cultureInfo.TextInfo.ToTitleCase(lastName);

        if (notifications.Count > 0)
        {
            return new LastNameValueObject(
                isValid: false,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
        else
        {
            return new LastNameValueObject(
                isValid: true,
                lastName: lastNameTitleCase,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
    }

    public string GetLastName()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return LastName;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;

}
