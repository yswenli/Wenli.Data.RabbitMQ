using System;
using System.Collections.Concurrent;
using Wenli.Data.RabbitMQ.Interface;

namespace Wenli.Data.RabbitMQ.Core
{
    public class ConnectionPool
    {
        private MQConfig _config;

        private ConcurrentQueue<IMQConnection> _connections = new ConcurrentQueue<IMQConnection>();

        internal ConnectionPool(MQConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
            for (int i = 0; i < config.PoolSize; i++)
            {
                FreeConnection(NewConnection());
            }
        }

        public IMQConnection GetConnection()
        {
            IMQConnection conn;
            while (_connections.TryDequeue(out conn))
            {
                if (conn.Connected)
                    return conn;
                else
                    conn.Dispose();
            }
            return NewConnection();
        }

        public void FreeConnection(IMQConnection conn)
        {
            _connections.Enqueue(conn);
        }

        private IMQConnection NewConnection()
        {
            return new MQConnection(_config);
        }
    }
}
