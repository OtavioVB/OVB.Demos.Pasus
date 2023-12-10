
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OVB.Demos.Eschody.Application;
using OVB.Demos.Eschody.Infrascructure;
using OVB.Demos.Eschody.Libraries.Observability;
using System.Text;
using System.Text.Json.Serialization;

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
            applicationName: builder.Configuration["Application:TenantInfo:Name"]!,
            rabbitMqClientProviderName: builder.Configuration["Application:TenantInfo:Name"]!,
            rabbitMqHostName: builder.Configuration["Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Hostname"] 
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Hostname\"]"),
            rabbitMqUsername: builder.Configuration["Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Username"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Username\"]"),
            rabbitVirtualHost: builder.Configuration["Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Virtualhost"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Virtualhost\"]"),
            rabbitMqPassword: builder.Configuration["Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Password"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Password\"]"),
            rabbitMqPort: Convert.ToInt32(builder.Configuration["Application:TenantInfo:Infrascructure:Messenger:RabbitMq:Port"]));

        #endregion

        #region Application Dependencies Configuration

        builder.Services.ApplyApplicationDependenciesConfiguration(
            authorizationPrivateToken: builder.Configuration["Application:TenantInfo:Authorization:PrivateToken"]
                ?? throw new ArgumentNullException("builder.Configuration[\"Application:TenantInfo:Authorization:PrivateToken\"]"));

        #endregion

        #region Authentication & Authorization Configuration

        builder.Services.AddAuthentication(p =>
        {
            p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(p =>
        {
            p.RequireHttpsMetadata = false;
            p.SaveToken = true;
            p.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    key: Encoding.ASCII.GetBytes(builder.Configuration["Application:TenantInfo:Authorization:PrivateToken"]!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        #endregion

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
