using Microsoft.Extensions.DependencyInjection;
using OVB.Demos.Eschody.Infrascructure.RabbitMq.Configuration;
using OVB.Demos.Eschody.Infrascructure.RabbitMq.Configuration.Interfaces;

namespace OVB.Demos.Eschody.Infrascructure.RabbitMq;

public static class DependencyInjection
{
    public static void ApplyRabbitMqMessengerDependenciesConfiguration(this IServiceCollection serviceCollection,
        string rabbitMqHostName,
        string rabbitMqVirtualHost,
        int rabbitMqPort,
        string rabbitMqClientProviderName,
        string rabbitMqUsername,
        string rabbitMqPassword)
    {
        #region RabbitMq Channel Configuration

        serviceCollection.AddSingleton<IRabbitMqConfiguration, RabbitMqConfiguration>(
            p => new RabbitMqConfiguration(
                hostname: rabbitMqHostName,
                virtualhost: rabbitMqVirtualHost,
                port: rabbitMqPort,
                clientProviderName: rabbitMqClientProviderName,
                username: rabbitMqUsername,
                password: rabbitMqPassword));

        #endregion
    }
}
