using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OVB.Demos.Eschody.Libraries.Observability.Trace;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Libraries.Observability;

public static class DependencyInjection
{
    public static void ApplyObservabilityDependenciesConfiguration(
        this IServiceCollection serviceCollection,
        string applicationName, 
        string applicationVersion,
        string applicationId,
        string applicationNamespace,
        string openTelemetryGrpcEndpoint,
        int openTelemetryTimeout)
    {

        #region Open Telemetry Dependencies Configuration

        serviceCollection.AddOpenTelemetry()
            .WithTracing(p =>
            {
                p.AddAspNetCoreInstrumentation();
                p.AddSource(applicationName);
                p.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                    serviceName: applicationName,
                    serviceVersion: applicationVersion,
                    serviceNamespace: applicationNamespace,
                    autoGenerateServiceInstanceId: false,
                    serviceInstanceId: applicationId));
                p.AddOtlpExporter(p =>
                {
                    p.ExportProcessorType = ExportProcessorType.Batch;
                    p.Endpoint = new Uri(
                        uriString: openTelemetryGrpcEndpoint);
                    p.Protocol = OtlpExportProtocol.Grpc;
                    p.TimeoutMilliseconds = openTelemetryTimeout;
                });
            })
            .WithMetrics(p =>
            {
                p.AddMeter(applicationName);
                p.AddAspNetCoreInstrumentation();
                p.AddOtlpExporter(p =>
                {
                    p.ExportProcessorType = ExportProcessorType.Batch;
                    p.Endpoint = new Uri(
                        uriString: openTelemetryGrpcEndpoint);
                    p.Protocol = OtlpExportProtocol.Grpc;
                    p.TimeoutMilliseconds = openTelemetryTimeout;
                });
            });

        #endregion

        #region Tracing Manager Depedencies Configuration

        serviceCollection.AddSingleton<ITraceManager, TraceManager>(
            serviceProvider => new TraceManager(
                activitySource: new ActivitySource(
                    name: applicationName,
                    version: applicationVersion)));

        #endregion
    }
}
