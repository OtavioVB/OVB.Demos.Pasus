using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

namespace OVB.Demos.Eschody.Domain.ValueObjects;

public readonly struct TenantScopeValueObject
{
    public bool IsValid { get; }
    private string Scope { get; }
    private ProcessResult<Notification> ProcessResult { get; }

    private TenantScopeValueObject(bool isValid, string scope, ProcessResult<Notification> processResult)
    {
        IsValid = isValid;
        Scope = scope;
        ProcessResult = processResult;
    }

    public static string AuthorizationScopeKey = "Scopes";

    public static TenantScopeValueObject Build(string scope)
    {
        var scopesExists = new List<string>()
        {
            "student.create",
            "student.read",
            "tenant.create",
            "tenant.read"
        };

        var hasNotifications = false;
        var notifications = new List<Notification>(1);

        var scopes = scope.Split(" ");

        foreach (var uniqueScope in scopes)
        {
            if (uniqueScope is null)
            {
                hasNotifications = true;
                notifications.Add(NotificationFacilitator.ScopeValid);
                break;
            }

            if (scopesExists.Any(p => p == uniqueScope) == false)
            {
                hasNotifications = true;
                notifications.Add(NotificationFacilitator.ScopeValid);
                break;
            }
        }

        if (!hasNotifications)
            return new TenantScopeValueObject(
                isValid: true,
                scope: scope,
                processResult: ProcessResult<Notification>.BuildSuccessfullProcessResult(
                    notifications: null));
        else
            return new TenantScopeValueObject(
                isValid: false,
                scope: string.Empty,
                processResult: ProcessResult<Notification>.BuildErrorfullProcessResult(
                    notifications: notifications.ToArray()));
    }

    public string GetScope()
    {
        PasusValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Scope;
    }

    public ProcessResult<Notification> GetProcessResult()
        => ProcessResult;
}
