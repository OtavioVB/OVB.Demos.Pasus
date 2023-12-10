using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization.Interfaces;

namespace OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization;

public sealed class AuthorizationManager : IAuthorizationManager
{
    public string PrivateToken { get; }
    public string CreateTenantKey { get; }

    private AuthorizationManager(string privateToken, string createTenantKey)
    {
        PrivateToken = privateToken;
        CreateTenantKey = createTenantKey;
    }

    public const string CreateTenantAuthorizationKey = "X-Pasus-Key";

    public static AuthorizationManager Build(string privateToken, string createTenantKey)
        => new AuthorizationManager(privateToken, createTenantKey);
}
