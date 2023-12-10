using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct EmailValueObject
{
    public bool IsValid { get; }
    public string Email { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private EmailValueObject(
        bool isValid, string email, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Email = email;
        ProcessResult = processResult;
    }

    private EmailValueObject(bool isValid, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Email = string.Empty;
        ProcessResult = processResult;
    }

    public const int EmailMaxLength = 255;
    public const int EmailMinLength = 5;

    public static EmailValueObject Build(string email)
    {
        var notifications = new List<Notification>();

        if (email.Length > EmailMaxLength)
            notifications.Add(
                item: NotificationFacilitator.EmailMaxLength);

        if (email.Length < EmailMinLength)
            notifications.Add(
                item: NotificationFacilitator.EmailMinLength);

        if (!Regex.IsMatch(email, "^[a-z0-9.-]+@[a-z0-9]+\\.[a-z]+(\\.[a-z]+)?$", RegexOptions.None, TimeSpan.FromMilliseconds(200)))
            notifications.Add(
                item: NotificationFacilitator.EmailValid);


        if (notifications.Count > 0)
        {
            return new EmailValueObject(
                isValid: false,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
        else
        {
            return new EmailValueObject(
                isValid: true,
                email: email,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
    }

    public string GetEmail()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Email;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;

}