using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Outputs;
using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Outputs;

public readonly struct OAuthTenantAuthenticationServiceResult
{
    private OAuthTenantAuthenticationServiceResult(GrantTypeValueObject grantType, TenantScopeValueObject scope, string type, string accessToken, int expiresIn)
    {
        GrantType = grantType;
        Scope = scope;
        Type = type;
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
    }

    public GrantTypeValueObject GrantType { get; }
    public TenantScopeValueObject Scope { get; }
    public string Type { get; }
    public string AccessToken { get; }
    public int ExpiresIn { get; }

    public static OAuthTenantAuthenticationServiceResult Build(GrantTypeValueObject grantType, TenantScopeValueObject scope, string type, string accessToken, int expiresIn)
        => new OAuthTenantAuthenticationServiceResult(grantType, scope, type, accessToken, expiresIn);

    public OAuthTenantAuthenticationUseCaseResult Adapt()
        => OAuthTenantAuthenticationUseCaseResult.Build(GrantType, Scope, Type, AccessToken, ExpiresIn);
}
