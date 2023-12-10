using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Outputs;

public readonly struct CreateTenantDomainFunctionResult
{
    private CreateTenantDomainFunctionResult(TenantCredentialsValueObject tenantCredentials, AuditableInfoValueObject auditableInfo, EmailValueObject email, PasswordValueObject password, ComercialNameValueObject comercialName, SocialReasonValueObject socialReason, CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj, CompositionValueObject composition, TenantScopeValueObject scope, FoundationDateValueObject foundationDate, DateTime isAvailableUntil, bool isTenantEnabled)
    {
        TenantCredentials = tenantCredentials;
        AuditableInfo = auditableInfo;
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
        IsTenantEnabled = isTenantEnabled;
    }

    public TenantCredentialsValueObject TenantCredentials { get; }
    public AuditableInfoValueObject AuditableInfo { get; }
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
    public bool IsTenantEnabled { get; }

    public static CreateTenantDomainFunctionResult Build(TenantCredentialsValueObject tenantCredentials, AuditableInfoValueObject auditableInfo, EmailValueObject email, 
        PasswordValueObject password, ComercialNameValueObject comercialName, SocialReasonValueObject socialReason, 
        CnaeCodeValueObject primaryCnaeCode, CnpjValueObject cnpj, CompositionValueObject composition, TenantScopeValueObject scope, 
        FoundationDateValueObject foundationDate, DateTime isAvailableUntil, bool isTenantEnabled)
        => new CreateTenantDomainFunctionResult(tenantCredentials, auditableInfo, email, password, comercialName, socialReason, primaryCnaeCode, 
            cnpj, composition, scope, foundationDate, isAvailableUntil, isTenantEnabled);
}
