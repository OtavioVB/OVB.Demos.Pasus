using Microsoft.AspNetCore.Connections;
using OVB.Demos.Eschody.Infrascructure.RabbitMq.Configuration.Interfaces;
using RabbitMQ.Client;

namespace OVB.Demos.Eschody.Infrascructure.RabbitMq.Configuration;

public sealed class RabbitMqConfiguration : IRabbitMqConfiguration
{
    private IModel? RabbitMqChannel { get; set; }

    private string Hostname { get; }
    private string Virtualhost { get; }
    private int Port { get; }
    private string ClientProviderName { get; }
    private string Username { get; }
    private string Password { get; }

    public RabbitMqConfiguration(
        string hostname, string virtualhost, int port, string clientProviderName, string username, string password)
    {
        Hostname = hostname;
        Virtualhost = virtualhost;
        Port = port;
        ClientProviderName = clientProviderName;
        Username = username;
        Password = password;
    }

    public IModel GetChannel()
    {
        if (RabbitMqChannel is null)
        {
            var factory = new ConnectionFactory()
            {
                Port = Port,
                HostName = Hostname,
                VirtualHost = Virtualhost,
                UserName = Username,
                ClientProvidedName = ClientProviderName,
                Password = Password,
                DispatchConsumersAsync = true
            };
            var rabbitMqConnection = factory.CreateConnection();
            RabbitMqChannel = rabbitMqConnection.CreateModel();
        }

        return RabbitMqChannel;
    }
}
