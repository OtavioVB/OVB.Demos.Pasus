namespace OVB.Demos.Eschody.WebApi.Controllers.TenantContext.Payloads;

public readonly struct CreateTenantPayloadInput
{
    public CreateTenantPayloadInput(string email, string password, string comercialName, string socialReason, string primaryCnaeCode, 
        string cnpj, int composition, string scope, DateTime foundationDate)
    {
        Email = email;
        Password = password;
        ComercialName = comercialName;
        SocialReason = socialReason;
        PrimaryCnaeCode = primaryCnaeCode;
        Cnpj = cnpj;
        Composition = composition;
        Scope = scope;
        FoundationDate = foundationDate;
    }

    public string Email { get; init; }
    public string Password { get; init; }
    public string ComercialName { get; init; }
    public string SocialReason { get; init; }
    public string PrimaryCnaeCode { get; init; }
    public string Cnpj { get; init; }
    public int Composition { get; init; }
    public string Scope { get; init; }
    public DateTime FoundationDate { get; init; }
}
