using System.Collections.Generic;
using RabbitMQ.Client;

namespace Wenli.Data.RabbitMQ.Interface
{
    public interface ISubscriber
    {
        void BasicAck(ulong deliveryTag, bool multiple);
        void BasicCancel(string consumerTag);
        string BasicConsume(string queue, bool noAck, IBasicConsumer consumer);
        string BasicConsume(string queue, bool noAck, string consumerTag, bool noLocal, bool exclusive, IDictionary<string, object> arguments, IBasicConsumer consumer);
        string BasicConsume(string queue, bool noAck, string consumerTag, IBasicConsumer consumer);
        string BasicConsume(string queue, bool noAck, string consumerTag, IDictionary<string, object> arguments, IBasicConsumer consumer);
        void BasicReject(ulong deliveryTag, bool requeue);
        void HandleBasicCancel(string consumerTag);
        void HandleBasicCancelOk(string consumerTag);
        void HandleBasicConsumeOk(string consumerTag);
        void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body);
        void HandleModelShutdown(object model, ShutdownEventArgs reason);
        void OnCancel();
        void QueueBind(string queue, string exchange, string routingKey);
        QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments);
    }
}