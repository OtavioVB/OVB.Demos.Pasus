using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Outputs;

public readonly struct OAuthTenantAuthenticationDomainFunctionResult
{
    private OAuthTenantAuthenticationDomainFunctionResult(
        GrantTypeValueObject grantType, TenantScopeValueObject scope, string type, string accessToken, int expiresIn)
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

    public static OAuthTenantAuthenticationDomainFunctionResult Build(
        GrantTypeValueObject grantType, TenantScopeValueObject scope, string type, string accessToken, int expiresIn)
        => new OAuthTenantAuthenticationDomainFunctionResult(grantType, scope, type, accessToken, expiresIn);
}
