using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Inputs;

public readonly struct OAuthTenantAuthenticationDomainFunctionInput
{
    private OAuthTenantAuthenticationDomainFunctionInput(TenantScopeValueObject scope, TenantCredentialsValueObject credentials, GrantTypeValueObject grantType)
    {
        Scope = scope;
        Credentials = credentials;
        GrantType = grantType;
    }

    public TenantScopeValueObject Scope { get; }
    public TenantCredentialsValueObject Credentials { get; }
    public GrantTypeValueObject GrantType { get; }

    public static OAuthTenantAuthenticationDomainFunctionInput Build(
        TenantScopeValueObject scope,
        TenantCredentialsValueObject credentials,
        GrantTypeValueObject grantType)
        => new OAuthTenantAuthenticationDomainFunctionInput(scope, credentials, grantType);
}
