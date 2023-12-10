using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Outputs;
using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Outputs;

public readonly struct OAuthTenantAuthenticationUseCaseResult
{
    private OAuthTenantAuthenticationUseCaseResult(GrantTypeValueObject grantType, TenantScopeValueObject scope, string type, string accessToken, int expiresIn)
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

    public static OAuthTenantAuthenticationUseCaseResult Build(GrantTypeValueObject grantType, TenantScopeValueObject scope, string type, string accessToken, int expiresIn)
        => new OAuthTenantAuthenticationUseCaseResult(grantType, scope, type, accessToken, expiresIn);
}
