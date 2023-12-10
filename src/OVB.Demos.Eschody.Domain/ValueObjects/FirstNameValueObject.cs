using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Globalization;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct FirstNameValueObject
{
    public bool IsValid { get; }
    public string FirstName { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private FirstNameValueObject(
        bool isValid, string firstName, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        FirstName = firstName;
        ProcessResult = processResult;
    }

    private FirstNameValueObject(bool isValid, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        FirstName = string.Empty;
        ProcessResult = processResult;
    }

    public const int FirstNameMaxLength = 32;
    public const int FirstNameMinLength = 2;

    public static FirstNameValueObject Build(string firstName)
    {
        var notifications = new List<Notification>();

        if (firstName.Length > FirstNameMaxLength)
            notifications.Add(NotificationFacilitator.FirstNameMaxLength);

        if (firstName.Length < FirstNameMinLength)
            notifications.Add(NotificationFacilitator.FirstNameMinLength);

        var cultureInfo = CultureInfo.GetCultureInfo("pt-BR");
        var firstNameTitleCase = cultureInfo.TextInfo.ToTitleCase(firstName);

        if (notifications.Count > 0)
        {
            return new FirstNameValueObject(
                isValid: false,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
        else
        {
            return new FirstNameValueObject(
                isValid: true,
                firstName: firstNameTitleCase,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
    }

    public string GetFirstName()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return FirstName;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;

}
