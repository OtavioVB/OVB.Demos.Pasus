using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization.Interfaces;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Inputs;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Interfaces;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Outputs;
using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.TenantContext.Entities;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Inputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Interfaces;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Inputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Interfaces;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Application.Services.Internal.TenantContext;

public sealed class TenantService : ITenantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Tenant> _tenantBaseRepository;
    private readonly IExtensionTenantRepository _extensionTenantRepository;
    private readonly ITraceManager _traceManager;
    private readonly IAuthorizationManager _authorizationManager;

    public TenantService(
        IUnitOfWork unitOfWork, 
        IBaseRepository<Tenant> tenantBaseRepository, 
        IExtensionTenantRepository extensionTenantRepository, 
        ITraceManager traceManager, 
        IAuthorizationManager authorizationManager)
    {
        _unitOfWork = unitOfWork;
        _tenantBaseRepository = tenantBaseRepository;
        _extensionTenantRepository = extensionTenantRepository;
        _traceManager = traceManager;
        _authorizationManager = authorizationManager;
    }

    public Task<ProcessResult<Notification, CreateTenantServiceResult>> CreateTenantServiceAsync(
        CreateTenantServiceInput input,
        AuditableInfoValueObject auditableInfo,
        CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(TenantService)}.{nameof(CreateTenantServiceAsync)}",
            activityKind: ActivityKind.Internal,
            input: input,
            handler: async (input, auditableInfo, activity, cancellationToken) =>
            {
                ICreateTenantDomainFunction tenantDomain = TenantStandard.Build(
                    privateToken: _authorizationManager.PrivateToken);
                var createTenantDomainResult = await tenantDomain.CreateTenantDomainFunctionAsync(
                    input: CreateTenantDomainFunctionInput.Build(
                        email: input.Email,
                        password: input.Password,
                        comercialName: input.ComercialName,
                        socialReason: input.SocialReason,
                        primaryCnaeCode: input.PrimaryCnaeCode,
                        cnpj: input.Cnpj,
                        composition: input.Composition,
                        scope: input.Scope,
                        foundationDate: input.FoundationDate),
                    auditableInfo: auditableInfo,
                    verifyTenantExistsByCnpj: _extensionTenantRepository.VerifyTenantExistsByCnpjAsync,
                    cancellationToken: cancellationToken);

                if (createTenantDomainResult.IsError)
                    return ProcessResult<Notification, CreateTenantServiceResult>.BuildErrorfullProcessResult(
                        output: default,
                        notifications: createTenantDomainResult.Notifications,
                        exceptions: createTenantDomainResult.Exceptions);

                if (createTenantDomainResult.IsPartial)
                    throw new NotImplementedException();

                await _tenantBaseRepository.AddAsync(
                    entity: createTenantDomainResult.Output.Adapt(),
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);
                await _unitOfWork.ApplyDatabaseTransactionAsync(cancellationToken);

                return ProcessResult<Notification, CreateTenantServiceResult>.BuildSuccessfullProcessResult(
                    output: CreateTenantServiceResult.Build(
                        credentials: createTenantDomainResult.Output.TenantCredentials,
                        email: createTenantDomainResult.Output.Email,
                        password: createTenantDomainResult.Output.Password,
                        comercialName: createTenantDomainResult.Output.ComercialName,
                        socialReason: createTenantDomainResult.Output.SocialReason,
                        primaryCnaeCode: createTenantDomainResult.Output.PrimaryCnaeCode,
                        cnpj: createTenantDomainResult.Output.Cnpj,
                        composition: createTenantDomainResult.Output.Composition,
                        scope: createTenantDomainResult.Output.Scope,
                        foundationDate: createTenantDomainResult.Output.FoundationDate,
                        isAvailableUntil: createTenantDomainResult.Output.IsAvailableUntil,
                        isEnabled: createTenantDomainResult.Output.IsTenantEnabled),
                    notifications: createTenantDomainResult.Notifications,
                    exceptions: createTenantDomainResult.Exceptions);
            },
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);

    public Task<ProcessResult<Notification, OAuthTenantAuthenticationServiceResult>> OAuthTenantAuthenticationServiceAsync(
        OAuthTenantAuthenticationServiceInput input, 
        AuditableInfoValueObject auditableInfo, 
        CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(TenantService)}.{nameof(CreateTenantServiceAsync)}",
            activityKind: ActivityKind.Internal,
            input: input,
            handler: async (input, auditableInfo, activity, cancellationToken) =>
            {
                IOAuthTenantAuthenticationDomainFunction tenantDomain = TenantStandard.Build(
                    privateToken: _authorizationManager.PrivateToken);

                var oauthTenantAuthenticationResult = await tenantDomain.OAuthTenantAuthenticationDomainFunctionAsync(
                    input: OAuthTenantAuthenticationDomainFunctionInput.Build(
                        scope: input.Scope,
                        credentials: input.Credentials,
                        grantType: input.GrantType),
                    auditableInfo: auditableInfo,
                    getTenantFromClientId: _extensionTenantRepository.GetTenantByClientIdAsync,
                    cancellationToken: cancellationToken);

                if (oauthTenantAuthenticationResult.IsError)
                    return ProcessResult<Notification, OAuthTenantAuthenticationServiceResult>.BuildErrorfullProcessResult(
                        output: default,
                        notifications: oauthTenantAuthenticationResult.Notifications,
                        exceptions: oauthTenantAuthenticationResult.Exceptions);

                if (oauthTenantAuthenticationResult.IsPartial)
                    throw new NotImplementedException();

                throw new NotImplementedException();
            },
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
}
