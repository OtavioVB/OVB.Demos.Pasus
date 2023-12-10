using OVB.Demos.Eschody.Libraries.NotificationContext;

namespace OVB.Demos.Eschody.WebApi.Controllers.TenantContext.Sendloads;

public readonly struct OAuthTenantAuthenticationSendload
{
    public OAuthTenantAuthenticationSendload(
        string accessToken, string type, string grantType, string scope, int expiresIn, Notification[]? notifications)
    {
        AccessToken = accessToken;
        Type = type;
        GrantType = grantType;
        Scope = scope;
        ExpiresIn = expiresIn;
        Notifications = notifications;
    }

    public string AccessToken { get; }
    public string Type { get; }
    public string GrantType { get; }
    public string Scope { get; }
    public int ExpiresIn { get; }
    public Notification[]? Notifications { get; }

}
