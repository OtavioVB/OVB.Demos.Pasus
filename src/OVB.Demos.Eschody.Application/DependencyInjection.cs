using Microsoft.Extensions.DependencyInjection;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Outputs;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;

namespace OVB.Demos.Eschody.Application;

public static class DependencyInjection
{
    public static void ApplyApplicationDependenciesConfiguration(
        this IServiceCollection serviceCollection)
    {
        #region Services Configuration

        serviceCollection.AddScoped<IStudentService, StudentService>();

        #endregion

        #region Use Cases Configuration

        serviceCollection.AddScoped<IUseCase<CreateStudentUseCaseInput, CreateStudentUseCaseResult>, CreateStudentUseCase>();

        #endregion
    }
}
