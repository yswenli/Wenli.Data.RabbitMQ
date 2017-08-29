using RabbitMQ.Client;
using Wenli.Data.RabbitMQ.Interface;

namespace Wenli.Data.RabbitMQ.Core
{
    public class Publisher : IPublisher
    {
        IModel _model;


        public Publisher(IModel model)
        {
            _model = model;
        }

        public void BasicPublish(PublicationAddress addr, IBasicProperties basicProperties, byte[] body)
        {
            _model.BasicPublish(addr, basicProperties, body);
        }
        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="basicProperties"></param>
        /// <param name="body"></param>
        public void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body)
        {
            //  默认使用强制入队模式，防止消息丢失
            _model.BasicPublish(exchange, routingKey, true, basicProperties, body);
        }
        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="mandatory">强制入队</param>
        /// <param name="basicProperties"></param>
        /// <param name="body"></param>
        public void BasicPublish(string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, byte[] body)
        {
            _model.BasicPublish(exchange, routingKey, mandatory, basicProperties, body);
        }

    }
}
