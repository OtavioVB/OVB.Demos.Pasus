using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Security.Cryptography;
using System.Text;
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
    public const int PasswordEncryptLength = 64;

    public static PasswordValueObject Build(string password, bool isEncrypted)
    {
        var notifications = new List<Notification>();

        if (isEncrypted)
        {
            return new PasswordValueObject(
                isValid: true,
                password: password,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }

        if (password.Length > PasswordMaxLength)
            notifications.Add(
                item: NotificationFacilitator.PasswordMaxLength);

        if (password.Length < PasswordMinLength)
            notifications.Add(
                item: NotificationFacilitator.PasswordMinLength);

        if (notifications.Count > 0)
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
                password: EncryptInformation(password),
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: notifications.ToArray(),
                    exceptions: null));
        }
    }

    private static string EncryptInformation(string text)
    {
        var encodedValue = Encoding.UTF8.GetBytes(text);
        var encryptedPassword = SHA256.HashData(encodedValue);
        var sb = new StringBuilder();
        foreach (var caracter in encryptedPassword) sb.Append(caracter.ToString("X2"));
        return sb.ToString();
    }

    public string GetPassword()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Password;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;

}
