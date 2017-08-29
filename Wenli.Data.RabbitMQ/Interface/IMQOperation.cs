using System;
using RabbitMQ.Client.Events;

namespace Wenli.Data.RabbitMQ.Interface
{
    public interface IMQOperation
    {
        ISubscriber ConsumerChannel { get; }

        void BasicAck(ulong deliveryTag, bool multiple);
        void Cancel(string queue);
        void Consume(string queue, Action<BasicDeliverEventArgs> action);
        void Enqueue(byte[] data);
        byte[] Dnqueue();
        T Dnqueue<T>();
        void Enqueue<T>(T t);
        void Subscribe();
        void UnSubscribe();
    }
}