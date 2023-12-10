using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct CnaeCodeValueObject
{
    public bool IsValid { get; }
    private string CnaeCode { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private CnaeCodeValueObject(bool isValid, string cnaeCode, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        CnaeCode = cnaeCode;
        ProcessResult = processResult;
    }

    public const int Length = 7;
    public const bool OnlyDigits = true;

    public static CnaeCodeValueObject Build(string cnaeCode)
    {
        var hasNotification = false;
        var notifications = new List<Notification>(2);

        if (cnaeCode.Length != Length)
        {
            notifications.Add(NotificationFacilitator.CnaeCodeLength);
            hasNotification = true;
        }

        foreach (var character in cnaeCode.ToArray())
        {
            if (char.IsDigit(character) == false)
            {
                notifications.Add(NotificationFacilitator.CnaeCodeDigit);
                hasNotification = true;
                break;
            }
        }

        if (hasNotification == true)
            return new CnaeCodeValueObject(
                isValid: false,
                cnaeCode: string.Empty,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray()));
        else
            return new CnaeCodeValueObject(
                isValid: true,
                cnaeCode: cnaeCode,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult());
    }

    public string GetCnaeCode()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return CnaeCode;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
