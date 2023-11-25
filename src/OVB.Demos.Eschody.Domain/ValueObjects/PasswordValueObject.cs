using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct PasswordValueObject
{
    public bool IsValid { get; }
    public string Password { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private PasswordValueObject(
        bool isValid, string password, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Password = password;
        ProcessResult = processResult;
    }

    private PasswordValueObject(bool isValid, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Password = string.Empty;
        ProcessResult = processResult;
    }

    public const int PasswordMaxLength = 32;
    public const int PasswordMinLength = 6;

    public static PasswordValueObject Build(string password)
    {
        var notifications = new List<Notification>();

        if (password.Length > PasswordMaxLength)
            notifications.Add(
                item: Notification.BuildErrorfullNotification(
                    code: "ESC10",
                    message: $"A senha precisa conter até {PasswordMaxLength} caracteres."));

        if (password.Length < PasswordMinLength)
            notifications.Add(
                item: Notification.BuildErrorfullNotification(
                    code: "ESC11",
                    message: $"A senha precisa conter pelo menos {PasswordMinLength} caracteres."));

        if (notifications.Count() > 0)
        {
            return new PasswordValueObject(
                isValid: false,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
        else
        {
            return new PasswordValueObject(
                isValid: true,
                password: password,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
    }

    public string GetPassword()
    {
        EschodyValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Password;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;

}
