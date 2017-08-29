using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using Wenli.Data.RabbitMQ.Handler;
using Wenli.Data.RabbitMQ.Interface;

namespace Wenli.Data.RabbitMQ.Core
{
    public class MQConnection : IDisposable, IMQConnection
    {

        protected ConnectionFactory ConnectionFactory { get; private set; }

        public IConnection Connection { get; private set; }

        public IModel Model { get; private set; }

        public bool Connected => Model != null && !Model.IsClosed;


        public MQConfig MQConfig { get; private set; }


        public IPublisher Publisher { get; private set; }


        public ISubscriber Subscriber { get; private set; }


        public IMQOperation GetOperation(string topic)
        {
            return new MQOperation(this, topic);
        }

        /// <summary>
        /// 指定配置节
        /// </summary>
        /// <param name="configSection"></param>
        public MQConnection(string configSection) : this(MQConfig.GetConfig(configSection))
        {

        }
        /// <summary>
        /// 指定配置
        /// </summary>
        /// <param name="config"></param>
        public MQConnection(MQConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            this.MQConfig = config;

            ConnectionFactory = new ConnectionFactory();
            ConnectionFactory.HostName = MQConfig.Server;
            ConnectionFactory.Port = MQConfig.Port;
            ConnectionFactory.UserName = MQConfig.User;
            ConnectionFactory.Password = MQConfig.Password;
            ConnectionFactory.VirtualHost = MQConfig.VirtualHost;
            ConnectionFactory.RequestedHeartbeat = 30;
            ConnectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(1);
            ConnectionFactory.AutomaticRecoveryEnabled = false;
            ConnectionFactory.TopologyRecoveryEnabled = false;
            Connection = ConnectionFactory.CreateConnection();
            //Connected = true;
            Connection.ConnectionShutdown += Connection_ConnectionShutdown;
            Connection.ConnectionBlocked += Connection_ConnectionBlocked;
            Connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
            Connection.CallbackException += Connection_CallbackException;
            Model = Connection.CreateModel();
            //Connection.AutoClose = true;
            Model.CallbackException += Model_CallbackException;
            Model.BasicReturn += Model_BasicReturn;
            Model.ExchangeDeclare(MQConfig.RouteName, MQConfig.RouteType, true, false, null);

            this.Publisher = new Publisher(this.Model);
            this.Subscriber = new Subscriber(this.Model);
        }


        public event ConnectionShutdown OnConnectionShutdown;

        public event ConnectionBlocked OnConnectionBlocked;

        public event ConnectionUnblocked OnConnectionUnblocked;

        public event CallbackException OnCallbackException;


        public event ModelCallbackException OnModelCallbackException;

        public event BasicReturn OnBasicReturn;


        private void Model_BasicReturn(object sender, global::RabbitMQ.Client.Events.BasicReturnEventArgs e)
        {
            OnBasicReturn?.Invoke(e);
        }

        private void Model_CallbackException(object sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            OnModelCallbackException?.Invoke(e);
        }

        private void Connection_CallbackException(object sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            OnCallbackException?.Invoke(e);
        }

        private void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            OnConnectionUnblocked(e);
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            OnConnectionShutdown?.Invoke(e);
        }

        private void Connection_ConnectionBlocked(object sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            OnConnectionBlocked(e);
        }

        public void Dispose()
        {
            try
            {
                Connection?.Abort(1000);
            }
            catch (AlreadyClosedException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
