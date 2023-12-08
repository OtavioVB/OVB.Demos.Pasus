
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
            applicationName: builder.Configuration["Application:TenantInfo:Name"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Name\"]"),
            applicationVersion: builder.Configuration["Application:TenantInfo:Version"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Version\"]"),
            applicationId: builder.Configuration["Application:TenantInfo:Id"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Id\"]"),
            applicationNamespace: builder.Configuration["Application:TenantInfo:Namespace"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Namespace\"]"),
            openTelemetryGrpcEndpoint: builder.Configuration["Application:TenantInfo:Infrascructure:OpenTelemetry:Endpoint"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:OpenTelemetry:Endpoint\"]"),
            openTelemetryTimeout: Convert.ToInt32(builder.Configuration["Application:TenantInfo:Infrascructure:OpenTelemetry:Timeout"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:OpenTelemetry:Timeout\"]")));

        #endregion

        #region Infrascructure Dependencies Configuration

        builder.Services.ApplyInfrascructureDependenciesConfiguration(
            connectionString: builder.Configuration["Application:TenantInfo:Infrascructure:Database:PostgreeSQL:ConnectionString"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:Database:PostgreeSQL:ConnectionString\"]"),
            redisConnectionString: builder.Configuration["Application:TenantInfo:Infrascructure:Database:Redis:ConnectionString"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:Database:Redis:ConnectionString\"]"),
            applicationName: builder.Configuration["Application:TenantInfo:Name"]!);

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
