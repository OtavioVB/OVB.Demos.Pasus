using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct PhoneValueObject
{
    public bool IsValid { get; }
    public string Phone { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private PhoneValueObject(
        bool isValid, string phone, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Phone = phone;
        ProcessResult = processResult;
    }

    private PhoneValueObject(bool isValid, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Phone = string.Empty;
        ProcessResult = processResult;
    }

    public const int PhoneLength = 11;

    public static PhoneValueObject Build(string phone)
    {
        var notifications = new List<Notification>();

        if (phone.Length != PhoneLength)
            notifications.Add(
                item: Notification.BuildErrorfullNotification(
                    code: "ESC08",
                    message: $"O telefone precisa conter até {PhoneLength} dígitos."));

        foreach (var character in phone)
        {
            if (!char.IsDigit(character))
            {
                notifications.Add(
                    item: Notification.BuildErrorfullNotification(
                        code: "ESC09",
                        message: $"O telefone deve conter apenas dígitos."));
            }
        }

        if (notifications.Count > 0)
        {
            return new PhoneValueObject(
                isValid: false,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
        else
        {
            return new PhoneValueObject(
                isValid: true,
                phone: phone,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
    }

    public string GetPhone()
    {
        EschodyValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Phone;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;

}
