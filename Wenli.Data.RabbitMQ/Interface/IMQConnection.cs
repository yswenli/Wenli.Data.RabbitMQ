using RabbitMQ.Client;
using Wenli.Data.RabbitMQ.Core;
using Wenli.Data.RabbitMQ.Handler;

namespace Wenli.Data.RabbitMQ.Interface
{
    public interface IMQConnection
    {
        IModel Model { get; }

        IConnection Connection { get; }

        ISubscriber Subscriber { get; }

        MQConfig MQConfig { get; }

        IPublisher Publisher { get; }


        IMQOperation GetOperation(string topic);


        bool Connected { get; }

        event BasicReturn OnBasicReturn;
        event CallbackException OnCallbackException;
        event ConnectionBlocked OnConnectionBlocked;
        event ConnectionShutdown OnConnectionShutdown;
        event ConnectionUnblocked OnConnectionUnblocked;
        event ModelCallbackException OnModelCallbackException;

        void Dispose();
    }
}