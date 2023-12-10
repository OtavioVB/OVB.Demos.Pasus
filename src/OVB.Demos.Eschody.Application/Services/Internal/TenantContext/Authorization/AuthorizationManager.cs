using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization.Interfaces;

namespace OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization;

public sealed class AuthorizationManager : IAuthorizationManager
{
    public string PrivateToken { get; }

    private AuthorizationManager(string privateToken)
    {
        PrivateToken = privateToken;
    }

    public static AuthorizationManager Build(string privateToken)
        => new AuthorizationManager(privateToken);
}
