using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Outputs;
using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Outputs;

public readonly struct CreateTenantServiceResult
{
    private CreateTenantServiceResult(TenantCredentialsValueObject credentials, EmailValueObject email, PasswordValueObject password, ComercialNameValueObject comercialName, SocialReasonValueObject socialReason, CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj, CompositionValueObject composition, TenantScopeValueObject scope, FoundationDateValueObject foundationDate, DateTime isAvailableUntil, bool isEnabled)
    {
        Credentials = credentials;
        Email = email;
        Password = password;
        ComercialName = comercialName;
        SocialReason = socialReason;
        PrimaryCnaeCode = primaryCnaeCode;
        Cnpj = cnpj;
        Composition = composition;
        Scope = scope;
        FoundationDate = foundationDate;
        IsAvailableUntil = isAvailableUntil;
        IsEnabled = isEnabled;
    }

    public TenantCredentialsValueObject Credentials { get; }
    public EmailValueObject Email { get; }
    public PasswordValueObject Password { get; }
    public ComercialNameValueObject ComercialName { get; }
    public SocialReasonValueObject SocialReason { get; }
    public CnaeCodeValueObject PrimaryCnaeCode { get; }
    public CnpjValueObject Cnpj { get; }
    public CompositionValueObject Composition { get; }
    public TenantScopeValueObject Scope { get; }
    public FoundationDateValueObject FoundationDate { get; }
    public DateTime IsAvailableUntil { get; }
    public bool IsEnabled { get; }

    public static CreateTenantServiceResult Build(TenantCredentialsValueObject credentials, EmailValueObject email, PasswordValueObject password, ComercialNameValueObject comercialName, SocialReasonValueObject socialReason, CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj, CompositionValueObject composition, TenantScopeValueObject scope, FoundationDateValueObject foundationDate, DateTime isAvailableUntil, bool isEnabled)
        => new CreateTenantServiceResult(credentials, email, password, comercialName, socialReason, primaryCnaeCode, cnpj, composition, scope, foundationDate, isAvailableUntil, isEnabled);

    public CreateTenantUseCaseResult Adapt()
        => CreateTenantUseCaseResult.Build(Credentials, Email, Password, ComercialName, SocialReason, PrimaryCnaeCode, Cnpj, Composition, Scope, FoundationDate, IsAvailableUntil, IsEnabled);

}
