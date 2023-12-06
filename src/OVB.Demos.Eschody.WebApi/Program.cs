
using OVB.Demos.Eschody.Application;
using OVB.Demos.Eschody.Infrascructure;
using OVB.Demos.Eschody.Libraries.Observability;

namespace OVB.Demos.Eschody.WebApi;

public partial class Program
{
    protected Program(){}

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Observability CrossCutting Dependencies Configuration

        builder.Services.ApplyObservabilityDependenciesConfiguration(
            applicationName: string.Empty,
            applicationVersion: string.Empty,
            applicationId: string.Empty,
            applicationNamespace: string.Empty,
            openTelemetryGrpcEndpoint: string.Empty,
            openTelemetryTimeout: 0);

        #endregion

        #region Infrascructure Dependencies Configuration

        builder.Services.ApplyInfrascructureDependenciesConfiguration(
            connectionString: string.Empty);

        #endregion

        #region Application Dependencies Configuration

        builder.Services.ApplyApplicationDependenciesConfiguration();

        #endregion

        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();
        app.Run();
    }
}
