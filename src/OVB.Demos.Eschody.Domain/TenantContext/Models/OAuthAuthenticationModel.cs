namespace OVB.Demos.Eschody.Domain.TenantContext.Models;

public readonly struct OAuthAuthenticationModel
{
    public string Type { get; }
    public string AccessToken { get; }
    public int ExpiresIn { get; }

    private OAuthAuthenticationModel(string type, string accessToken, int expiresIn)
    {
        Type = type;
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
    }

    public static OAuthAuthenticationModel Build(string type, string accessToken, int expiresIn)
        => new OAuthAuthenticationModel(type, accessToken, expiresIn);
}
