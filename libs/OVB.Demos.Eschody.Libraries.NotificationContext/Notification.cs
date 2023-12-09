using OVB.Demos.Eschody.Libraries.NotificationContext.Enums;
using System.Text.Json.Serialization;

namespace OVB.Demos.Eschody.Libraries.NotificationContext;

public readonly struct Notification
{
    public Notification(string code, string message, TypeNotification type, int? index = null)
    {
        Code = code;
        Message = message;
        Type = type.ToString();
        Index = index;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Index { get; }
    public string Code { get; }
    public string Message { get; }
    public string Type { get; }

    public static Notification BuildNotification(string code, string message, TypeNotification type, int? index = null)
        => new Notification(code, message, type, index);

    public static Notification BuildErrorfullNotification(string code, string message, int? index = null)
        => new Notification(code, message, TypeNotification.Errorfull, index);

    public static Notification BuildSuccessfullNotification(string code, string message, int? index = null)
        => new Notification(code, message, TypeNotification.Successfull, index);

    public static Notification BuildInformationNotification(string code, string message, int? index = null)
        => new Notification(code, message, TypeNotification.Information, index);
    public static Notification ParseNotificationWithoutIndexWithIndex(Notification notification, int index)
        => new Notification(notification.Code, notification.Message, ParseStringToTypeNotificationEnum(notification.Type), index);
    public static Notification[] ParseNotificationsWithoutIndexWithIndex(Notification[] notifications, int index)
    {
        var notificationsWithIndex = new Notification[notifications.Length];

        for (int i = 0; i < notifications.Length; i++)
        {
            notificationsWithIndex[i] = ParseNotificationWithoutIndexWithIndex(notifications[i], index);
        }

        return notificationsWithIndex;
    }
    public static TypeNotification ParseStringToTypeNotificationEnum(string text)
    {
        switch (text)
        {
            case "Errorfull":
                return TypeNotification.Errorfull;
            case "Successfull":
                return TypeNotification.Successfull;
            case "Information":
                return TypeNotification.Information;
            default:
                throw new NotImplementedException();
        }
    }
}