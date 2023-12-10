using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Inputs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using static System.Formats.Asn1.AsnWriter;

namespace OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Inputs;

public readonly struct CreateTenantUseCaseInput
{
    private CreateTenantUseCaseInput(EmailValueObject email, PasswordValueObject password, ComercialNameValueObject comercialName, SocialReasonValueObject socialReason, CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj, CompositionValueObject composition, TenantScopeValueObject scope, FoundationDateValueObject foundationDate)
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

    public EmailValueObject Email { get; }
    public PasswordValueObject Password { get; }
    public ComercialNameValueObject ComercialName { get; }
    public SocialReasonValueObject SocialReason { get; }
    public CnaeCodeValueObject PrimaryCnaeCode { get; }
    public CnpjValueObject Cnpj { get; }
    public CompositionValueObject Composition { get; }
    public TenantScopeValueObject Scope { get; }
    public FoundationDateValueObject FoundationDate { get; }

    public static CreateTenantUseCaseInput Build(EmailValueObject email, PasswordValueObject password, ComercialNameValueObject comercialName, SocialReasonValueObject socialReason, CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj, CompositionValueObject composition, TenantScopeValueObject scope, FoundationDateValueObject foundationDate)
        => new CreateTenantUseCaseInput(email, password, comercialName, socialReason, primaryCnaeCode, cnpj, composition, scope, foundationDate);
    public CreateTenantServiceInput Adapt()
        => CreateTenantServiceInput.Build(Email, Password, ComercialName, SocialReason, PrimaryCnaeCode, Cnpj, Composition, Scope, FoundationDate);

}