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
