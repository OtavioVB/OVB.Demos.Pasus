using Microsoft.IdentityModel.Tokens;
using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Inputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Interfaces;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.CreateTenant.Outputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Inputs;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Interfaces;
using OVB.Demos.Eschody.Domain.TenantContext.Functions.OAuthTenantAuthentication.Outputs;
using OVB.Demos.Eschody.Domain.TenantContext.Models;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace OVB.Demos.Eschody.Domain.TenantContext.Entities;

public class TenantStandard : ICreateTenantDomainFunction, IOAuthTenantAuthenticationDomainFunction
{
    private readonly string _privateToken;

    private TenantStandard(string privateToken)
        => _privateToken = privateToken;

    public static TenantStandard Build(string privateToken)
        => new TenantStandard(privateToken);

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

        var requiredValidation = ValidateThatTenantDoesNotExistsYet(tenantExists);

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

    public async Task<ProcessResult<Notification, OAuthTenantAuthenticationDomainFunctionResult>> OAuthTenantAuthenticationDomainFunctionAsync(
        OAuthTenantAuthenticationDomainFunctionInput input,
        AuditableInfoValueObject auditableInfo,
        Func<Guid, AuditableInfoValueObject, CancellationToken, Task<Tenant?>> getTenantFromClientId, 
        CancellationToken cancellationToken)
    {
        var initialValidation = ProcessResult<Notification>.BuildFromAnotherProcessResult(
            input.Credentials.GetProcessResult(), input.GrantType.GetProcessResult(), input.Scope.GetProcessResult());

        if (initialValidation.IsError)
            return ProcessResult<Notification, OAuthTenantAuthenticationDomainFunctionResult>.BuildErrorfullProcessResult(
                output: default,
                notifications: initialValidation.Notifications,
                exceptions: initialValidation.Exceptions);

        if (initialValidation.IsPartial)
            throw new NotImplementedException();

        var tenant = await getTenantFromClientId(
            arg1: input.Credentials.GetClientId(),
            arg2: auditableInfo,
            arg3: cancellationToken);

        var validateTenantExists = ValidateThatTenantIsNotNull(tenant);

        if (validateTenantExists.IsError)
            return ProcessResult<Notification, OAuthTenantAuthenticationDomainFunctionResult>.BuildErrorfullProcessResult(
                output: default,
                notifications: validateTenantExists.Notifications,
                exceptions: validateTenantExists.Exceptions);

        if (validateTenantExists.IsPartial)
            throw new NotImplementedException();

        var validateTenantClientSecret = ValidateThatClientSecretIsValid(
            credentials: input.Credentials,
            clientSecret: tenant!.ClientSecret);

        if (validateTenantClientSecret.IsError)
            return ProcessResult<Notification, OAuthTenantAuthenticationDomainFunctionResult>.BuildErrorfullProcessResult(
                output: default,
                notifications: validateTenantClientSecret.Notifications,
                exceptions: validateTenantClientSecret.Exceptions);

        if (validateTenantClientSecret.IsPartial)
            throw new NotImplementedException();

        var validateTenantDisponibility = ProcessResult<Notification>.BuildFromAnotherProcessResult(
            ValidateThatTenantIsAvailable(tenant.IsTenantAvailableUntil),
            ValidateThatTenantIsEnabled(tenant.IsTenantEnabled),
            ValidateThatTenantIsEnabledToScope(tenant.Scope, input.Scope.GetScope()));

        if (validateTenantDisponibility.IsError)
            return ProcessResult<Notification, OAuthTenantAuthenticationDomainFunctionResult>.BuildErrorfullProcessResult(
                output: default,
                notifications: validateTenantDisponibility.Notifications,
                exceptions: validateTenantDisponibility.Exceptions);

        if (validateTenantDisponibility.IsPartial)
            throw new NotImplementedException();

        var authorization = OAuthAuthenticationDomain(
            tenant: tenant,
            auditableInfo: auditableInfo,
            scope: input.Scope,
            grantType: input.GrantType);

        return ProcessResult<Notification, OAuthTenantAuthenticationDomainFunctionResult>.BuildSuccessfullProcessResult(
            output: OAuthTenantAuthenticationDomainFunctionResult.Build(
                grantType: input.GrantType,
                scope: input.Scope,
                type: authorization.Type,
                accessToken: authorization.AccessToken,
                expiresIn: authorization.ExpiresIn));
    }

    protected virtual OAuthAuthenticationModel OAuthAuthenticationDomain(
        Tenant tenant, AuditableInfoValueObject auditableInfo, TenantScopeValueObject scope, GrantTypeValueObject grantType)
    {
        const string oauthTokenType = "Bearer";
        const int oauthTokenExpiresIn = 3600;

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_privateToken);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Tenant"),
                new Claim("ClientId", tenant.ClientId.ToString()),
                new Claim("Scopes", scope.GetScope()),
                new Claim("GrantType", grantType.GetGrantType()),
                new Claim("AuthCorrelationId", auditableInfo.GetCorrelationId().ToString()),
                new Claim("AuthAuthorizerUser", auditableInfo.GetSourcePlatform().ToString()),
                new Claim("AuthTargetPlatform", auditableInfo.GetExecutionUser().ToString())
            }),
            Expires = DateTime.UtcNow.AddSeconds(oauthTokenExpiresIn),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return OAuthAuthenticationModel.Build(
            accessToken: tokenHandler.WriteToken(token),
            type: oauthTokenType,
            expiresIn: oauthTokenExpiresIn);
    }


    protected virtual ProcessResult<Notification> ValidateThatTenantIsEnabledToScope(string scopeRegistered, string scopeProvided)
    {
        var scopesRegistered = scopeRegistered.Split(' ');
        var scopesProvided = scopeRegistered.Split(' ');

        for (int i = 0; i < scopesProvided.Length; i++)
        {
            var hasPermission = false;

            for (int j = 0; j < scopesRegistered.Length; j++)
            {
                if (scopeProvided[i] == scopeRegistered[j])
                    hasPermission = true;
            }

            if (!hasPermission)
            {
                return ProcessResult<Notification>.BuildErrorfullProcessResult(
                   notifications: [
                       NotificationFacilitator.TenantScopeIsNotValid(scopesProvided[i])
                   ],
                   exceptions: null);
            }
        }

        return ProcessResult<Notification>.BuildSuccessfullProcessResult();
    }
    protected virtual ProcessResult<Notification> ValidateThatTenantIsEnabled(bool enabled)
    {
        if (enabled == false)
            return ProcessResult<Notification>.BuildErrorfullProcessResult(
               notifications: [
                   NotificationFacilitator.TenantIsEnabled
               ],
               exceptions: null);
        else
            return ProcessResult<Notification>.BuildSuccessfullProcessResult();
    }

    protected virtual ProcessResult<Notification> ValidateThatTenantIsAvailable(DateTime isAvailableUntil)
    {
        if (isAvailableUntil < DateTime.UtcNow)
            return ProcessResult<Notification>.BuildErrorfullProcessResult(
               notifications: [
                   NotificationFacilitator.TenantIsNotAvailable(isAvailableUntil)
               ],
               exceptions: null);
        else
            return ProcessResult<Notification>.BuildSuccessfullProcessResult();
    }

    protected virtual ProcessResult<Notification> ValidateThatClientSecretIsValid(TenantCredentialsValueObject credentials, Guid clientSecret)
    {
        if (credentials.GetClientSecret() != clientSecret)
            return ProcessResult<Notification>.BuildErrorfullProcessResult(
               notifications: [
                   NotificationFacilitator.TenantClientSecretInvalid
               ],
               exceptions: null);
        else
            return ProcessResult<Notification>.BuildSuccessfullProcessResult();
    }

    protected virtual ProcessResult<Notification> ValidateThatTenantIsNotNull<TEntity>(TEntity? entity)
    {
        if (entity == null)
            return ProcessResult<Notification>.BuildErrorfullProcessResult(
               notifications: [
                   NotificationFacilitator.TenantNotExists
               ],
               exceptions: null);
        else
            return ProcessResult<Notification>.BuildSuccessfullProcessResult();
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
            return ProcessResult<Notification>.BuildSuccessfullProcessResult();
    }
}
