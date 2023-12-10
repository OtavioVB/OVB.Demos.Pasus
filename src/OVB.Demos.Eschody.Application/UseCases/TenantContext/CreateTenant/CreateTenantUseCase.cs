using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Inputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Outputs;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant;

public sealed class CreateTenantUseCase : IUseCase<CreateTenantUseCaseInput, CreateTenantUseCaseResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITraceManager _traceManager;
    private readonly ITenantService _tenantService;

    public CreateTenantUseCase(
        IUnitOfWork unitOfWork, 
        ITraceManager traceManager, 
        ITenantService tenantService)
    {
        _unitOfWork = unitOfWork;
        _traceManager = traceManager;
        _tenantService = tenantService;
    }

    public Task<ProcessResult<Notification, CreateTenantUseCaseResult>> ExecuteUseCaseAsync(
        CreateTenantUseCaseInput input, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: nameof(CreateTenantUseCase),
            activityKind: ActivityKind.Internal,
            input: input,
            handler: (input, auditableInfo, activity, cancellationToken) =>
                _unitOfWork.ExecuteUnitOfWorkAsync(
                    async (cancellationToken) =>
                    {
                        var createTenantServiceResult = await _tenantService.CreateTenantServiceAsync(
                            input: input.Adapt(),
                            auditableInfo: auditableInfo,
                            cancellationToken: cancellationToken);

                        if (createTenantServiceResult.IsError)
                            return (false, ProcessResult<Notification, CreateTenantUseCaseResult>.BuildErrorfullProcessResult(
                                output: default,
                                notifications: createTenantServiceResult.Notifications,
                                exceptions: createTenantServiceResult.Exceptions));

                        if (createTenantServiceResult.IsPartial)
                            throw new NotImplementedException();

                        return (true, ProcessResult<Notification, CreateTenantUseCaseResult>.BuildSuccessfullProcessResult(
                                output: createTenantServiceResult.Output.Adapt(),
                                notifications: createTenantServiceResult.Notifications,
                                exceptions: createTenantServiceResult.Exceptions));
                    }, cancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
}
