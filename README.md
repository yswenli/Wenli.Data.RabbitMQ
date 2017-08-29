# Wenli.Data.RabbitMQ
Wenli.Data.RabbitMQ


<img src="https://github.com/yswenli/Wenli.Data.RabbitMQ/blob/master/Wenli.Data.RabbitMQ.Console/QQ%E4%BA%94%E7%AC%94%E6%88%AA%E5%9B%BE%E6%9C%AA%E5%91%BD%E5%90%8D.png?raw=true" alt="wenli.data.rabbitmq"/>

配置节点如下：

&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
&lt;configuration&gt;
  &lt;configSections&gt;
    &lt;section name=&quot;MQConfig&quot; type=&quot;Wenli.Data.RabbitMQ.MQConfig,Wenli.Data.RabbitMQ&quot;/&gt;
  &lt;/configSections&gt;
  &lt;MQConfig Server=&quot;127.0.0.1&quot; Port=&quot;5672&quot; User=&quot;wenli&quot; Password=&quot;wenli&quot; RouteName=&quot;IMMQ&quot; RouteType=&quot;direct&quot; VirtualHost=&quot;/&quot;  QosCount=&quot;20000&quot;/&gt;
  &lt;startup&gt;
    &lt;supportedRuntime version=&quot;v4.0&quot; sku=&quot;.NETFramework,Version=v4.5&quot; /&gt;
  &lt;/startup&gt;
&lt;/configuration&gt;


示例代码如下：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wenli.Data.RabbitMQ.Console
{
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Wenli.Data.RabbitMQ.Console";
            Console.WriteLine("正连接到mq");

            try
            {
                Test();
            }
            catch (Exception ex)
            {
                Console.WriteLine("err:" + ex.Message + ex.Source + ex.StackTrace);
            }

            Console.Read();
        }


        static void Test()
        {

            var topic = "testtopic";

            var cnn = RabbitMQBuilder.Get(MQConfig.Default).GetConnection();

            var operation = cnn.GetOperation(topic);

            Console.WriteLine("正连接到订阅【" + topic + "】");

            operation.Subscribe();

            Console.WriteLine("正在入队");

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    operation.Enqueue(Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "     hello!"));
                    Thread.Sleep(1);
                }
            });




            Console.WriteLine("正在出队");



            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var result = operation.Dnqueue();

                    if (result == null)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(result));
                    }
                }
            });

            Console.ReadLine();

            Console.WriteLine("正在取消订阅");

            operation.UnSubscribe();

            Console.WriteLine("测试完成");
        }
    }
}
