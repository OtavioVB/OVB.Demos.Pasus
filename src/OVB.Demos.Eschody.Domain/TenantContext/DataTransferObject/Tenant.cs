using OVB.Demos.Eschody.Domain.TenantContext.ENUMs;

namespace OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;

public sealed record Tenant
{
    public Tenant(
        Guid clientId, Guid clientSecret, 
        
        Guid correlationId, string sourcePlatform, string executionUser, DateTime createdAt, 
        
        string email, string comercialName, string socialReason, string cnpj, string password, DateTime foundationDate, 
        TypeTenantComposition composition, string primaryCnaeCode, string scope, DateTime isTenantAvailableUntil, bool isTenantEnabled, 
        
        Guid lastCorrelationId, string lastSourcePlatform, string lastExecutionUser, DateTime lastModifiedAt)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        CorrelationId = correlationId;
        SourcePlatform = sourcePlatform;
        ExecutionUser = executionUser;
        CreatedAt = createdAt;
        Email = email;
        ComercialName = comercialName;
        SocialReason = socialReason;
        Cnpj = cnpj;
        Password = password;
        FoundationDate = foundationDate;
        Composition = composition;
        PrimaryCnaeCode = primaryCnaeCode;
        Scope = scope;
        IsTenantAvailableUntil = isTenantAvailableUntil;
        IsTenantEnabled = isTenantEnabled;
        LastCorrelationId = lastCorrelationId;
        LastSourcePlatform = lastSourcePlatform;
        LastExecutionUser = lastExecutionUser;
        LastModifiedAt = lastModifiedAt;
    }

    public Guid ClientId { get; set; }
    public Guid ClientSecret { get; set; }

    public Guid CorrelationId { get; set; }
    public string SourcePlatform { get; set; }
    public string ExecutionUser { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Email { get; set; }
    public string ComercialName { get; set; }
    public string SocialReason { get; set; }
    public string Cnpj { get; set; }
    public string Password { get; set; }
    public DateTime FoundationDate { get; set; }
    public TypeTenantComposition Composition { get; set; }
    public string PrimaryCnaeCode { get; set; }
    public string Scope { get; set; }
    public DateTime IsTenantAvailableUntil { get; set; }
    public bool IsTenantEnabled { get; set; }

    public Guid LastCorrelationId { get; set; }
    public string LastSourcePlatform { get; set; }
    public string LastExecutionUser { get; set; }
    public DateTime LastModifiedAt { get; set; }
}
