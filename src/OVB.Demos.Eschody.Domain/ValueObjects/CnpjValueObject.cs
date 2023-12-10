using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct CnpjValueObject
{
    public bool IsValid { get; }
    private string Cnpj { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private CnpjValueObject(bool isValid, string cnpj, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Cnpj = cnpj;
        ProcessResult = processResult;
    }

    public const int CnpjLength = 14;
    public const bool HasOnlyNumbers = true;

    public static CnpjValueObject Build(string cnpj)
    {
        bool hasNotification = false;
        var notifications = new List<Notification>(2);

        if (cnpj.Length != CnpjLength)
        {
            notifications.Add(
                item: NotificationFacilitator.CnpjLength);
            hasNotification = true;
        }

        foreach (var character in cnpj.ToArray())
        {
            if (char.IsDigit(character) == false)
            {
                notifications.Add(
                item: NotificationFacilitator.CnpjDigit);
                hasNotification = true;
            }
        }

        if (hasNotification)
            return new CnpjValueObject(
                isValid: false,
                cnpj: string.Empty,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray()));
        else
            return new CnpjValueObject(
                isValid: true,
                cnpj: cnpj,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult());
    }

    public string GetCnpj()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Cnpj;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
