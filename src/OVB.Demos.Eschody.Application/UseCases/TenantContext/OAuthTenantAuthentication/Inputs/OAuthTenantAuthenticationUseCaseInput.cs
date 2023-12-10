using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Inputs;
using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Inputs;

public readonly struct OAuthTenantAuthenticationUseCaseInput
{
    private OAuthTenantAuthenticationUseCaseInput(TenantScopeValueObject scope, TenantCredentialsValueObject credentials, GrantTypeValueObject grantType)
    {
        Scope = scope;
        Credentials = credentials;
        GrantType = grantType;
    }

    public TenantScopeValueObject Scope { get; }
    public TenantCredentialsValueObject Credentials { get; }
    public GrantTypeValueObject GrantType { get; }

    public static OAuthTenantAuthenticationUseCaseInput Build(TenantScopeValueObject scope, TenantCredentialsValueObject credentials, GrantTypeValueObject grantType)
        => new OAuthTenantAuthenticationUseCaseInput(scope, credentials, grantType);

    public OAuthTenantAuthenticationServiceInput Adapt()
        => OAuthTenantAuthenticationServiceInput.Build(Scope, Credentials, GrantType);
}
