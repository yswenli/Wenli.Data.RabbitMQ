using RabbitMQ.Client;

namespace Wenli.Data.RabbitMQ.Interface
{
    public interface IPublisher
    {
        void BasicPublish(PublicationAddress addr, IBasicProperties basicProperties, byte[] body);
        void BasicPublish(string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, byte[] body);
        void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body);
    }
}