using Microsoft.AspNetCore.Authentication.OAuth;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Inputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Outputs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication;

public sealed class OAuthTenantAuthenticationUseCase : IUseCase<OAuthTenantAuthenticationUseCaseInput, OAuthTenantAuthenticationUseCaseResult>
{
    private readonly ITenantService _tenantService;
    private readonly ITraceManager _traceManager;

    public OAuthTenantAuthenticationUseCase(
        ITenantService tenantService, 
        ITraceManager traceManager)
    {
        _tenantService = tenantService;
        _traceManager = traceManager;
    }

    public Task<ProcessResult<Notification, OAuthTenantAuthenticationUseCaseResult>> ExecuteUseCaseAsync(
        OAuthTenantAuthenticationUseCaseInput input, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: nameof(OAuthTenantAuthenticationUseCase),
            activityKind: ActivityKind.Internal,
            input: input,
            handler: async (input, auditableInfo, activity, cancellationToken) =>
            {
                var oauthTenantAuthenticationServiceResult = await _tenantService.OAuthTenantAuthenticationServiceAsync(
                    input: input.Adapt(),
                    auditableInfo: auditableInfo,
                    cancellationToken: cancellationToken);

                if (oauthTenantAuthenticationServiceResult.IsError)
                    return ProcessResult<Notification, OAuthTenantAuthenticationUseCaseResult>.BuildErrorfullProcessResult(
                        output: default,
                        notifications: oauthTenantAuthenticationServiceResult.Notifications,
                        exceptions: oauthTenantAuthenticationServiceResult.Exceptions);

                if (oauthTenantAuthenticationServiceResult.IsPartial)
                    throw new NotImplementedException();

                return ProcessResult<Notification, OAuthTenantAuthenticationUseCaseResult>.BuildSuccessfullProcessResult(
                    output: oauthTenantAuthenticationServiceResult.Output.Adapt(),
                    notifications: oauthTenantAuthenticationServiceResult.Notifications,
                    exceptions: oauthTenantAuthenticationServiceResult.Exceptions);
            },
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
}
