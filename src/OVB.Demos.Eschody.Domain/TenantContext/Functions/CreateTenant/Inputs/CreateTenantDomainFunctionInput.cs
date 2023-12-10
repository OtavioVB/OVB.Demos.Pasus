using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Inputs;

public readonly struct CreateTenantDomainFunctionInput
{
    private CreateTenantDomainFunctionInput(
        EmailValueObject email, PasswordValueObject password, ComercialNameValueObject comercialName, 
        SocialReasonValueObject socialReason, CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj, 
        CompositionValueObject composition, TenantScopeValueObject scope, FoundationDateValueObject foundationDate)
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

    public static CreateTenantDomainFunctionInput Build(EmailValueObject email, PasswordValueObject password, ComercialNameValueObject comercialName,
        SocialReasonValueObject socialReason, CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj,
        CompositionValueObject composition, TenantScopeValueObject scope, FoundationDateValueObject foundationDate)
        => new CreateTenantDomainFunctionInput(email, password, comercialName, socialReason, primaryCnaeCode, cnpj, composition, scope, foundationDate);
}
