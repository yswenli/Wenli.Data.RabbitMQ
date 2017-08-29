using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wenli.Data.RabbitMQ.Core;

namespace Wenli.Data.RabbitMQ
{
    public static class RabbitMQBuilder
    {
        private static ConcurrentDictionary<string, ConnectionPool> _pools = new ConcurrentDictionary<string, ConnectionPool>();


        public static  ConnectionPool Get(MQConfig config)
        {
            return _pools.GetOrAdd(string.Format("{0}:{1}:{2}:{3}", config.SectionInformation.SectionName, config.VirtualHost, config.Server, config.Port), key => new ConnectionPool(config));
        }
    }
}
