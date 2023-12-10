namespace OVB.Demos.Eschody.WebApi.Controllers.TenantContext.Payloads;

public readonly struct OAuthTenantAuthenticationPayloadInput
{
    public OAuthTenantAuthenticationPayloadInput(string grantType, string scope)
    {
        GrantType = grantType;
        Scope = scope;
    }

    public string GrantType { get; init; }
    public string Scope { get; init; }
}
