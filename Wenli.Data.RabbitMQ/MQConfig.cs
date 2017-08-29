using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wenli.Data.RabbitMQ
{
    public class MQConfig : ConfigurationSection
    {
        /// <summary>
        /// 默认读取MQConfig
        /// </summary>
        public static MQConfig Default
        {
            get
            {
               return  MQConfig.GetConfig("MQConfig");
            }
        }

        /// <summary>
        /// RabbitMQ地址
        /// </summary>
        [ConfigurationProperty("Server", IsRequired = true)]
        public string Server
        {
            get { return (string)base["Server"]; }
            set { base["Server"] = value; }
        }
        /// <summary>
        /// RabbitMQ端口
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = false)]
        public int Port
        {
            get
            {
                var _port = (int)base["Port"];
                return _port > 0 ? _port : 5672;
            }
            set { base["Port"] = value; }
        }

        /// <summary>
        /// RabbitMQ登录用户
        /// </summary>
        [ConfigurationProperty("User", IsRequired = true)]
        public string User
        {
            get
            {
                return (string)base["User"];
            }
            set { base["User"] = value; }
        }
        /// <summary>
        /// RabbitMQ登录密码
        /// </summary>
        [ConfigurationProperty("Password", IsRequired = true)]
        public string Password
        {
            get
            {
                return (string)base["Password"];
            }
            set { base["Password"] = value; }
        }

        /// <summary>
        /// RabbitMQ路由名称
        /// </summary>
        [ConfigurationProperty("RouteName", IsRequired = true)]
        public string RouteName
        {
            get
            {
                return (string)base["RouteName"];
            }
            set { base["RouteName"] = value; }
        }
        /// <summary>
        /// RabbitMQ路由类型名称
        /// </summary>
        [ConfigurationProperty("RouteType", IsRequired = false)]
        public string RouteType
        {
            get
            {
                var _type = (string)base["RouteType"];
                return !string.IsNullOrEmpty(_type) ? _type : Wenli.Data.RabbitMQ.Common.MQRouteType.DirectExchange;
            }
            set { base["RouteType"] = value; }
        }
        /// <summary>
        /// RabbitMQ队列名称列表{"queueName:10","queueName:10"}
        /// queueName为队列名称，10代表同时出队数
        /// </summary>
        [ConfigurationProperty("Queues", IsRequired = false)]

        public string Queues
        {
            get
            {
                return (string)base["Queues"];
            }
            set { base["Queues"] = value; }
        }
        /// <summary>
        /// RabbitMQ不给ACK时的最大出队数，默认为10
        /// </summary>
        [ConfigurationProperty("QosCount", IsRequired = false)]
        public int QosCount
        {
            get
            {
                var _count = (int)base["QosCount"];
                return _count > 0 ? _count : 10;
            }
            set { base["QosCount"] = value; }
        }
        public static MQConfig GetConfig()
        {
            var section = (MQConfig)ConfigurationManager.GetSection("RabbitMQConfig");
            return section;
        }
        public static MQConfig GetConfig(string sectionName)
        {
            var section = (MQConfig)ConfigurationManager.GetSection(sectionName);
            //  跟默认配置相同的，可以省略
            if (section == null)
                section = GetConfig();
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            return section;
        }

        /// <summary>
        ///     连接池大小
        /// </summary>
        [ConfigurationProperty("PoolSize", IsRequired = false, DefaultValue = 0)]
        public int PoolSize
        {
            get
            {
                return (int)base["PoolSize"];
            }
            set
            {
                base["PoolSize"] = value;
            }
        }

        [ConfigurationProperty("VirtualHost", IsRequired = false, DefaultValue = "/")]
        public string VirtualHost
        {
            get { return (string)base["VirtualHost"]; }
            set { base["VirtualHost"] = value; }
        }
    }
}
