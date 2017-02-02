using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lind.DDD.Redis.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] emailServices = { "LessonOnLine", "Ordered", "MoneyIn", "MoneyOut", "zzl" };
            foreach (var service in emailServices)
            {
                Lind.DDD.PublishSubscribe.PubSubManager.Instance.Publish(service, "hello world");
                Thread.Sleep(1000);
            }
            Console.ReadKey();
        }
    }
}
