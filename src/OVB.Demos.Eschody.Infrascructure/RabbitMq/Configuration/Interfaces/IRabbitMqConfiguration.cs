using RabbitMQ.Client;

namespace OVB.Demos.Eschody.Infrascructure.RabbitMq.Configuration.Interfaces;

public interface IRabbitMqConfiguration
{
    public IModel GetChannel();
}
