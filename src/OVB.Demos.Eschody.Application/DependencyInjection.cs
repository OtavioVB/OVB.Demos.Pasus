using Microsoft.Extensions.DependencyInjection;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Interfaces;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Authorization.Interfaces;
using OVB.Demos.Eschody.Application.Services.Internal.TenantContext.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent;
using OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent.Outputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Inputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.CreateTenant.Outputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Inputs;
using OVB.Demos.Eschody.Application.UseCases.TenantContext.OAuthTenantAuthentication.Outputs;

namespace OVB.Demos.Eschody.Application;

public static class DependencyInjection
{
    public static void ApplyApplicationDependenciesConfiguration(
        this IServiceCollection serviceCollection,
        string authorizationPrivateToken,
        string basicAuthorization)
    {
        #region Authorization Manager Configuration

        serviceCollection.AddSingleton<IAuthorizationManager, AuthorizationManager>((serviceProvider) => 
            AuthorizationManager.Build(authorizationPrivateToken, basicAuthorization));

        #endregion

        #region Services Configuration

        serviceCollection.AddScoped<IStudentService, StudentService>();
        serviceCollection.AddScoped<ITenantService, TenantService>();

        #endregion

        #region Use Cases Configuration

        serviceCollection.AddScoped<IUseCase<CreateStudentUseCaseInput, CreateStudentUseCaseResult>, CreateStudentUseCase>();
        serviceCollection.AddScoped<IUseCase<CreateTenantUseCaseInput, CreateTenantUseCaseResult>, CreateTenantUseCase>();
        serviceCollection.AddScoped<IUseCase<OAuthTenantAuthenticationUseCaseInput, OAuthTenantAuthenticationUseCaseResult>, 
            OAuthTenantAuthenticationUseCase>();

        #endregion
    }
}
