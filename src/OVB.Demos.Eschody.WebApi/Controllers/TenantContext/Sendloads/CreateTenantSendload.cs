namespace OVB.Demos.Eschody.WebApi.Controllers.TenantContext.Sendloads;

public readonly struct CreateTenantSendload
{
    public CreateTenantSendload(Guid clientId, Guid clientSecret, string email, string cnpj, string comercialName, string socialReason, string primaryCnaeCode, int composition, string scope, string foundationDate, DateTime isAvailableUntil, bool isEnabled)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        Email = email;
        Cnpj = cnpj;
        ComercialName = comercialName;
        SocialReason = socialReason;
        PrimaryCnaeCode = primaryCnaeCode;
        Composition = composition;
        Scope = scope;
        FoundationDate = foundationDate;
        IsAvailableUntil = isAvailableUntil;
        IsEnabled = isEnabled;
    }

    public Guid ClientId { get; }
    public Guid ClientSecret { get; }
    public string Email { get; }
    public string Cnpj { get; }
    public string ComercialName { get; }
    public string SocialReason { get; }
    public string PrimaryCnaeCode { get; }
    public int Composition { get; }
    public string Scope { get; }
    public string FoundationDate { get; }
    public DateTime IsAvailableUntil { get; }
    public bool IsEnabled { get; }
}
