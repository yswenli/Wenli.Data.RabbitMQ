using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wenli.Data.RabbitMQ.Common;
using Wenli.Data.RabbitMQ.Interface;

namespace Wenli.Data.RabbitMQ.Core
{
    public class MQOperation : IMQOperation
    {
        string _topic = string.Empty;

        IMQConnection _cnn;

        private ConcurrentDictionary<string, Subscriber> _subscribers = new ConcurrentDictionary<string, Subscriber>();


        private ConcurrentQueue<BasicDeliverEventArgs> _cache = new ConcurrentQueue<BasicDeliverEventArgs>();

        ISubscriber _consumer;
        /// <summary>
        /// 消费者channel
        /// </summary>
        public ISubscriber ConsumerChannel
        {
            get
            {
                return _consumer;
            }
        }


        public MQOperation(IMQConnection cnn, string topic)
        {
            _cnn = cnn;
            _topic = topic;
            _consumer = cnn.Subscriber;
        }
        /// <summary>
        /// 消费队列数据
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="action"></param>
        public void Consume(string queue, Action<BasicDeliverEventArgs> action)
        {
            var consumer = new Subscriber(action);
            if (_subscribers.TryAdd(queue, consumer))
            {
                ConsumerChannel.QueueDeclare(queue, true, false, false, null);
                ConsumerChannel.QueueBind(queue, _cnn.MQConfig.RouteName, queue);
                consumer.ConsumerTag = _consumer.BasicConsume(queue, false/*noAck*/, consumer);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public virtual void Cancel(string queue)
        {
            Subscriber consumer;
            if (_subscribers.TryRemove(queue, out consumer))
            {
                _consumer.BasicCancel(consumer.ConsumerTag);
            }
        }


        /// <summary>
        /// 发送ack
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="multiple"></param>
        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            _consumer.BasicAck(deliveryTag, multiple);
        }


        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="buffer"></param>
        /// <param name="basicProperties"></param>
        protected void Enqueue(string queue, byte[] buffer, IBasicProperties basicProperties = null)
        {
            _cnn.Publisher.BasicPublish(_cnn.MQConfig.RouteName, queue, basicProperties, buffer);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="data"></param>
        public void Enqueue(byte[] data)
        {
            Enqueue(_topic, data);
        }
        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public byte[] Dnqueue()
        {
            BasicDeliverEventArgs args;
            if (_cache.TryDequeue(out args))
            {
                ConsumerChannel.BasicAck(args.DeliveryTag, false);
                return args.Body;
            }
            return null;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        public void Subscribe()
        {
            this.Consume(_topic, args =>
            {
                _cache.Enqueue(args);
            });
        }
        /// <summary>
        /// 取消
        /// </summary>
        public void UnSubscribe()
        {
            this.Cancel(_topic);
            BasicDeliverEventArgs args;
            while (_cache.TryDequeue(out args))
            {
                this.ConsumerChannel.BasicReject(args.DeliveryTag, true);
            }
        }

        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="t"></param>
        public void Enqueue<T>(T t)
        {
            Enqueue(_topic, SerializeUtil.ProtolBufSerialize(t));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Dnqueue<T>()
        {
            BasicDeliverEventArgs args;
            if (_cache.TryDequeue(out args))
            {
                ConsumerChannel.BasicAck(args.DeliveryTag, false);
                return SerializeUtil.ProtolBufDeserialize<T>(args.Body);
            }
            return default(T);
        }

    }
}
