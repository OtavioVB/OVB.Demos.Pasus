using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Inputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Interfaces;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Outputs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System;

namespace OVB.Demos.Eschody.Domain.TenantContext.Entities;

public class TenantStandard : ICreateTenantDomainFunction
{
    public static TenantStandard Build()
        => new TenantStandard();

    public virtual async Task<ProcessResult<Notification, CreateTenantDomainFunctionResult>> CreateTenantDomainFunctionAsync(
        CreateTenantDomainFunctionInput input, 
        AuditableInfoValueObject auditableInfo, 
        Func<string, AuditableInfoValueObject, CancellationToken, Task<bool>> verifyTenantExistsByCnpj, 
        CancellationToken cancellationToken)
    {
        var initialValidation = ProcessResult<Notification>.BuildFromAnotherProcessResult(
            input.Cnpj.GetProcessResult(), input.Email.GetProcessResult(), input.Password.GetProcessResult(),
            input.Composition.GetProcessResult(), input.Scope.GetProcessResult(), input.ComercialName.GetProcessResult(),
            input.SocialReason.GetProcessResult(), input.FoundationDate.GetProcessResult(),
            input.PrimaryCnaeCode.GetProcessResult());

        if (initialValidation.IsError)
            return ProcessResult<Notification, CreateTenantDomainFunctionResult>.BuildErrorfullProcessResult(
                output: default,
                notifications: initialValidation.Notifications,
                exceptions: initialValidation.Exceptions);

        if (initialValidation.IsPartial)
            throw new NotImplementedException();

        var tenantExists = await verifyTenantExistsByCnpj(
            arg1: input.Cnpj.GetCnpj(),
            arg2: auditableInfo,
            arg3: cancellationToken);

        var requiredValidation = ProcessResult<Notification>.BuildFromAnotherProcessResult(
            ValidateThatTenantDoesNotExistsYet(tenantExists));

        if (requiredValidation.IsError)
            return ProcessResult<Notification, CreateTenantDomainFunctionResult>.BuildErrorfullProcessResult(
                output: default,
                notifications: requiredValidation.Notifications,
                exceptions: requiredValidation.Exceptions);

        if (requiredValidation.IsPartial)
            throw new NotImplementedException();

        return ProcessResult<Notification, CreateTenantDomainFunctionResult>.BuildSuccessfullProcessResult(
            output: CreateTenantDomainFunctionResult.Build(
                tenantCredentials: TenantCredentialsValueObject.Build(),
                auditableInfo: auditableInfo,
                email: input.Email,
                password: input.Password,
                comercialName: input.ComercialName,
                socialReason: input.SocialReason,
                primaryCnaeCode: input.PrimaryCnaeCode,
                cnpj: input.Cnpj,
                composition: input.Composition,
                scope: input.Scope,
                foundationDate: input.FoundationDate,
                isAvailableUntil: DateTime.UtcNow.AddDays(31),
                isTenantEnabled: false));
    }

    protected virtual ProcessResult<Notification> ValidateThatTenantDoesNotExistsYet(bool tenantExists)
    {
        if (tenantExists)
            return ProcessResult<Notification>.BuildErrorfullProcessResult(
                notifications: [
                    NotificationFacilitator.TenantExistsDomain
                ],
                exceptions: null);
        else
            return ProcessResult<Notification>.BuildSuccessfullProcessResult(
                notifications: null,
                exceptions: null);
    }
}
