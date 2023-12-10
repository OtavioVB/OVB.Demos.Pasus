using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Inputs;

public readonly struct OAuthTenantAuthenticationServiceInput
{
    private OAuthTenantAuthenticationServiceInput(TenantScopeValueObject scope, TenantCredentialsValueObject credentials, GrantTypeValueObject grantType)
    {
        Scope = scope;
        Credentials = credentials;
        GrantType = grantType;
    }

    public TenantScopeValueObject Scope { get; }
    public TenantCredentialsValueObject Credentials { get; }
    public GrantTypeValueObject GrantType { get; }

    public static OAuthTenantAuthenticationServiceInput Build(TenantScopeValueObject scope, TenantCredentialsValueObject credentials, GrantTypeValueObject grantType)
        => new OAuthTenantAuthenticationServiceInput(scope, credentials, grantType);
}
